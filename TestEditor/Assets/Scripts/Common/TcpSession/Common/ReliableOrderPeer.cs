using System;
using System.Collections.Generic;
using NetWrapper.Network.Tool;


namespace NetWrapper.Network
{
    public class ReliableOrderPeer : PeerBase
    {
        private StreamBuffer m_reliableSendBuffer;
        private Queue<StreamBuffer> m_reliablBufferQueue;
        private int m_sendIndex;
        private DateTime m_sendTimePoint;
        private double m_sendTimeDis;

        public ReliableOrderPeer(IPeerListener peerListener, bool batch = false)
            : base(peerListener, batch)
        {
            InitPeer(null);
        }

        protected ReliableOrderPeer(IGameSession iSession, IPeerListener peerListener, bool batch = false)
            : base(peerListener, batch)
        {
            InitPeer(iSession);
        }

        protected void InitPeer(IGameSession iSession)
        {
            m_peerSession = iSession;
            m_reliableSendBuffer = new StreamBuffer(-1, 10240);
            m_reliablBufferQueue = new Queue<StreamBuffer>();
            m_sendIndex = 0;
            m_sendTimePoint = DateTime.Now;
            m_sendTimeDis = 200.0f;
        }

        protected override void OnReceivePacket(StreamBuffer sb)
        {
            if (sb.m_protocolHeader.ProtocolID == -2)
            {
                if (sb.m_protocolHeader.ErrorCode == m_reliableSendBuffer.m_protocolHeader.ErrorCode)
                {
                    m_reliableSendBuffer.ClearBuffer();
                }
            }
            else if (sb.m_protocolHeader.ProtocolID == -1)
            {
                StreamBuffer sendbuffer = new StreamBuffer(-2, 0);
                sendbuffer.m_protocolHeader.ErrorCode = sb.m_protocolHeader.ErrorCode;
                m_peerSession.Send(sendbuffer);

                RawStreamBuffer cReceiveBuffer = new RawStreamBuffer(sb.ByteBuffer);
                ProcessPacket(ref cReceiveBuffer);
            }
            else
            {
                m_peerListener.OnPktReceive(sb);
            }
        }

        public override void Connect(string address, int port)
        {
            if (m_peerSession == null)
            {
                m_peerSession = AsynTcpClient.Instance.InstanceSession(this);
                base.Connect(address, port);
            }
        }

        protected override void OnDisconnected()
        {
            if (m_peerSession != null)
            {
                AsynTcpClient.Instance.RemoveSession(m_peerSession.GetSessionID());
                m_peerSession = null;
            }

            base.OnDisconnected();
        }

        public void Send(DELIVERY_TYPE dType, StreamBuffer sb)
        {
            if (dType == DELIVERY_TYPE.RELIABLE_ORDERED)
            {
                m_reliablBufferQueue.Enqueue(sb);
            }
            else
            {
                Send(sb);
            }
        }

        private void ProcessPacket(ref RawStreamBuffer cReceiveBuffer)
        {
            while (cReceiveBuffer.WriteIndex - cReceiveBuffer.ReadIndex >= ProtocolHeader.HeadLength)
            {
                StreamBuffer sb = Packing.GetPacketBufferWithHeader(ref cReceiveBuffer);
                if (sb != null)
                {
                    m_peerListener.OnPktReceive(sb);
                }
                else
                {
                    break;
                }
            }
            //cReceiveBuffer.ClearBuffer();
        }

        protected override void OnDispatch()
        {
            base.OnDispatch();

            if (m_reliableSendBuffer.m_protocolHeader.BodyLength == 0)
            {
                if (m_reliablBufferQueue.Count > 0)
                {
                    while (m_reliablBufferQueue.Count > 0)
                    {
                        StreamBuffer buffer = m_reliablBufferQueue.Dequeue();
                        m_reliableSendBuffer.WriteBuffer(buffer);
                    }
                    m_reliableSendBuffer.m_protocolHeader.ErrorCode = m_sendIndex++;

                    if (SendBatchBuffer(m_reliableSendBuffer))
                    {
                        m_sendTimePoint = DateTime.Now;
                    }
                }
            }
            else
            {
                DateTime currentTime = DateTime.Now;
                if ((currentTime - m_sendTimePoint).TotalMilliseconds > m_sendTimeDis)
                {
                    if(SendBatchBuffer(m_reliableSendBuffer))
                    {
                        m_sendTimePoint = currentTime;
                    }
                }
            }
        }
    }
}