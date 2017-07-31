using System;

namespace NetWrapper.Network
{
    public class TPeer : PeerBase
    {
        private DateTime m_sendPingTime;
        private DateTime m_recPingTime;
        private bool m_connected;
        private bool m_bHeartBeat;
        private double m_sendPingTimeout;
        private double m_recPingTimeout;
        private readonly int m_csHeartBeat;
        private readonly int m_scHeartBeat;
         
        public TPeer(IPeerListener peerListener, bool batch = false)
            : base(peerListener, batch)
        {
            this.m_sendPingTime = System.DateTime.Now;
            this.m_recPingTime = System.DateTime.Now;
            this.m_sendPingTimeout = 0.0f;
            this.m_recPingTimeout = 0.0f;
            this.m_bHeartBeat = false;
            this.m_connected = false;
            this.m_csHeartBeat = 0x50a0005;
            this.m_scHeartBeat = 0x70a0005;
        }

        public void SetPingTime(double millisecond)
        {
            m_sendPingTimeout = millisecond;
            m_recPingTimeout = millisecond * 2.0f;
            m_bHeartBeat = m_sendPingTimeout > 0;
        }

        public override void Connect(string address, int port)
        {
            if (m_peerSession == null)
            {
                m_peerSession = AsynTcpClient.Instance.InstanceSession(this);
                base.Connect(address, port);
            }
        }
        private void CheckPingPacket()
        {
            DateTime currentTime = System.DateTime.Now;
            double elapseSend = (currentTime - this.m_sendPingTime).TotalMilliseconds;
            if (elapseSend >= m_sendPingTimeout)
            {
                StreamBuffer sb = new StreamBuffer(m_csHeartBeat, 0);
                m_peerSession.Send(sb);
                this.m_sendPingTime = currentTime;
            }

            double elapseRec = (currentTime - this.m_recPingTime).TotalMilliseconds;
            if (elapseRec > m_recPingTimeout)
            {
                Disconnect();
            }
        }
        protected override void OnDispatch()
        {
            base.OnDispatch();

            if (this.m_connected && this.m_bHeartBeat)
            {
                CheckPingPacket();
            }
        }
        protected override void OnConnected()
        {
            base.OnConnected();

            this.m_recPingTime = System.DateTime.Now;
            this.m_connected = true;
        }
        protected override void OnDisconnected()
        {
            if (m_peerSession != null)
            {
                AsynTcpClient.Instance.RemoveSession(m_peerSession.GetSessionID());
                m_peerSession = null;
            }

            base.OnDisconnected();

            this.m_connected = false;
        }
        protected override void OnReceivePacket(StreamBuffer sb)
        {
            if (sb.m_protocolHeader.ProtocolID == m_scHeartBeat)
            {
                m_recPingTime = System.DateTime.Now;
            }
            else
            {
                m_peerListener.OnPktReceive(sb);
            }
        }
    }
}