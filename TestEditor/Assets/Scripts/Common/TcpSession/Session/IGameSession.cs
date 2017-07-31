
//  IGameSession.cs
//  nilsen
//  2016-10-20
using System;
using System.Net;
using System.Net.Sockets;
using NetWrapper.Network.Tool;
using System.Collections.Generic;
using System.Threading;

namespace NetWrapper.Network
{
    /// <summary>
    /// 会话接口
    /// </summary>
    public abstract class IGameSession : ISession, IDisposable
    {
        private readonly object m_syncer = new object();
        protected Socket m_cSocket;
        private readonly int m_sessionID;
        private BlockQueue<RawStreamBuffer> m_sendBufferQueue;
        protected DispatchBase m_cDispatch;
        private Queue<Action> m_actionQueue;
        private Queue<Action> m_tmpQueue;
        private Dictionary<SocketState, Action> m_statusActions;
        protected SocketState m_SocketState;
        private string m_sAddress;
        private int m_port;
        private IPAddress m_connectIPAddress;
        protected Thread m_receiveThread;
        protected Thread m_sendThread;
        public IGameSession(int sessionID, ISessionListener sessionListener)
        {
            this.m_cSocket = null;
            this.m_sessionID = sessionID;
            this.m_sendBufferQueue = new BlockQueue<RawStreamBuffer>();
            this.m_cDispatch = new GameDispatch(sessionListener, this);
            this.m_actionQueue = new Queue<Action>();
            this.m_tmpQueue = new Queue<Action>();
            this.m_statusActions = new Dictionary<SocketState, Action>()
            {
                {SocketState.Connecting, DoConnecting},
                {SocketState.Connected, ()=> { m_cDispatch.OnConnect(); } },
                {SocketState.Disconnecting, DoDisConnecting},
                {SocketState.Disconnected, ()=>{ m_cDispatch.OnDisconnect(); } },
            };
            this.m_SocketState = SocketState.Noconnect;
            this.m_sAddress = string.Empty;
            this.m_port = 0;
            this.m_connectIPAddress = null;
            this.m_receiveThread = null;
            this.m_sendThread = null;
        }

        public void Dispose()
        {
            m_SocketState = SocketState.Disconnecting;
            m_sendBufferQueue.Close();
            if (this.m_cSocket != null)
            {
                try
                {
                    if (m_cSocket.Connected)
                    {
                        m_cSocket.Shutdown(SocketShutdown.Both);
                        m_cSocket.Close();
                    }
                }
                catch (Exception ex)
                {
                    WriteFiles.WritFile.Log(ex);
                }
            }
            m_cSocket = null;
            m_SocketState = SocketState.Disconnected;
        }

        public int GetSessionID()
        {
            return this.m_sessionID;
        }

        protected virtual void ChangeStatus(SocketState status)
        {
            this.m_SocketState = status;
            lock (m_actionQueue)
            {
                if (m_statusActions.ContainsKey(status))
                {
                    if (!m_actionQueue.Contains(m_statusActions[status]))
                    {
                        m_actionQueue.Enqueue(m_statusActions[status]);
                    }
                }
            }
        }

        public virtual void Connect(string address, int port)
        {
            if (m_SocketState != SocketState.Disconnected && m_SocketState != SocketState.Noconnect)
            {
                WriteFiles.WritFile.Log(LogerType.WARN, "Connect() failed: session in State: " + this.m_SocketState);
                return;
            }
            WriteFiles.WritFile.Log(LogerType.DEBUG, string.Format("try connect {0}:{1}", address, port));

            this.m_sAddress = address;
            this.m_port = port;
            this.m_connectIPAddress = null;

            ChangeStatus(SocketState.Connecting);
        }

        public bool Reconnect()
        {
            if (m_SocketState == SocketState.Disconnected && m_connectIPAddress != null)
            {
                ChangeStatus(SocketState.Connecting);
                return true;
            }

            return false;
        }

        public void ShutDown()
        {
            DoDisConnecting();
            Update();
        }

        private void DoConnecting()
        {
            object obj = this.m_syncer;
            lock (obj)
            {
                try
                {
                    if (m_SocketState == SocketState.Connected) return;

                    if (m_connectIPAddress == null)
                    {
                        IPAddress ipAddress = NetUtil.GetIPAddress(m_sAddress);
                        if (ipAddress == null)
                        {
                            throw new ArgumentException("Invalid IPAddress. Address: " + m_sAddress);
                        }

                        m_connectIPAddress = ipAddress;
                    }

                    if(m_connectIPAddress != null)
                    {
                        this.m_cSocket = null;
                        this.m_cSocket = new Socket(m_connectIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        if (this.m_cSocket == null)
                        {
                            throw new Exception("Socket create error.");
                        }
                        WriteFiles.WritFile.Log(LogerType.DEBUG, string.Format("Connect address {0}:{1}", m_connectIPAddress.ToString(), m_port));
                        this.m_cSocket.NoDelay = true;
                        this.m_cSocket.BeginConnect(m_connectIPAddress, m_port, new AsyncCallback(OnConnected), m_cSocket);
                    }
                    else
                    {
                        HandleException(SocketState.Disconnected);
                    }
                }
                catch (Exception e)
                {
                    WriteFiles.WritFile.Log(e);
                    HandleException(SocketState.Disconnected);
                    return;
                }
            }
        }

        private void OnConnected(IAsyncResult result)
        {
            try
            {
                Socket client = (Socket)result.AsyncState;

                this.m_SocketState = SocketState.Connected;
                ChangeStatus(SocketState.Connected);
                m_receiveThread = new Thread(new ThreadStart(ReceiveLoop)) { Name = m_sessionID.ToString() + " rec thread", IsBackground = true };
                m_receiveThread.Start();
                m_sendThread = new Thread(new ThreadStart(SendLoop)) { Name = m_sessionID.ToString() + " send thread" , IsBackground = true };
                m_sendThread.Start();

                client.EndConnect(result);
            }
            catch (Exception e)
            {
                WriteFiles.WritFile.Log(e);
                HandleException(SocketState.Disconnected);
            }
        }

        public virtual void DisConnect()
        {
            if (m_SocketState != SocketState.Connecting && m_SocketState != SocketState.Connected)
            {
                WriteFiles.WritFile.Log(LogerType.WARN, "DisConnect() failed: session in State: " + this.m_SocketState);
                return;
            }

            ChangeStatus(SocketState.Disconnecting);
        }

        private void DoDisConnecting()
        {
            object obj = this.m_syncer;
            lock (obj)
            {
                if (m_SocketState == SocketState.Disconnected) return;

                m_sendBufferQueue.Close();

                if (m_cSocket != null)
                {
                    try
                    {
                        if (this.m_cSocket.Connected)
                        {
                            this.m_cSocket.Shutdown(SocketShutdown.Both);
                        }

                        m_cSocket.Close();
                    }
                    catch (Exception e)
                    {
                        WriteFiles.WritFile.Log(e);
                    }
                    this.m_cSocket = null;
                }

                if (m_receiveThread != null)
                {
                    m_receiveThread.Join();
                    m_receiveThread = null;
                }
                if (m_sendThread != null)
                {
                    m_sendThread.Join();
                    m_sendThread = null;
                }
                m_SocketState = SocketState.Disconnected;
                ChangeStatus(SocketState.Disconnected);
            }
        }

        public virtual void Send(RawStreamBuffer sb)
        {
            if (m_SocketState == SocketState.Connected)
            {
                m_sendBufferQueue.Enqueue(sb);
            }
        }

        public virtual void Update()
        {
            this.m_cDispatch.Update();

            if (m_actionQueue.Count > 0)
            {
                lock (m_actionQueue)
                {
                    Queue<Action>.Enumerator iter = m_actionQueue.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        m_tmpQueue.Enqueue(iter.Current);
                    }
                    this.m_actionQueue.Clear();
                }

                while (m_tmpQueue.Count > 0)
                {
                    Action action = m_tmpQueue.Dequeue();
                    action();
                }
            }
        }

        protected void ReceiveLoop()
        {
            while (m_SocketState == SocketState.Connected)
            {
                try
                {
                    int recHeaderSize = 0;
                    byte[] headerStream = new byte[ProtocolHeader.HeadLength];
                    do
                    {
                        int recCount = this.m_cSocket.Receive(headerStream, recHeaderSize, ProtocolHeader.HeadLength - recHeaderSize, SocketFlags.None);
                        recHeaderSize += recCount;

                        if (recCount == 0)
                        {
                            WriteFiles.WritFile.Log(LogerType.INFO, string.Format("session be closed by peer when receive header"));

                            throw new SocketException((int)SocketError.ConnectionReset);
                        }
                    } while (recHeaderSize < ProtocolHeader.HeadLength);

                    ProtocolHeader pHeader = new ProtocolHeader(headerStream);
                    if (pHeader.BodyLength > ProtocolHeader.MaxHeadLength || pHeader.BodyLength < 0)
                    {
                        WriteFiles.WritFile.Log(LogerType.WARN, string.Format("session be closed by peer when receive, bodyLenght error {0}", pHeader.BodyLength));
                        continue;
                    }
                    byte[] bodyStream = null;
                    if(pHeader.BodyLength > 0)
                    {
                        int recBodySize = 0;
                        bodyStream = new byte[pHeader.BodyLength];
                        do
                        {
                            int recCount = this.m_cSocket.Receive(bodyStream, recBodySize, pHeader.BodyLength - recBodySize, SocketFlags.None);
                            recBodySize += recCount;

                            if (recCount == 0)
                            {
                                WriteFiles.WritFile.Log(LogerType.INFO, string.Format("session be closed by peer when receive body"));
                                throw new SocketException((int)SocketError.ConnectionReset);
                            }
                        } while (recBodySize < pHeader.BodyLength);
                    }

                    StreamBuffer recSB = new StreamBuffer(pHeader, bodyStream);
                    m_cDispatch.AckPacket(recSB);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        WriteFiles.WritFile.Log(LogerType.INFO, string.Format("session be closed by peer when receive (interruped)"));
                        HandleException(SocketState.Disconnecting);
                    }
                    else
                    {
                        WriteFiles.WritFile.Log(e);
                        HandleException(SocketState.Disconnecting);
                    }
                }
                catch (Exception ex)
                {
                    WriteFiles.WritFile.Log(ex);
                    HandleException(SocketState.Disconnecting);
                }
            }
        }

        protected void SendLoop()
        {
            while (!m_sendBufferQueue.Closed)
            {
                try
                {
                    RawStreamBuffer sb = m_sendBufferQueue.Dequeue();
                    if (sb == null)
                    {
                        continue;
                    }

                    byte[] sendBuf = null;
                    int sendLength = sb.GetSendBuffer(out sendBuf);
                    if (sendLength == 0)
                    {
                        continue;
                    }

                    if (this.m_SocketState == SocketState.Connected)
                    {
                        int sendBufferSize = 0;
                        do
                        {
                            int bytesSend = this.m_cSocket.Send(sendBuf, sendBufferSize, sendLength - sendBufferSize, SocketFlags.DontRoute);
                            sendBufferSize += bytesSend;

                            if (bytesSend <= 0)
                            {
                                WriteFiles.WritFile.Log(LogerType.INFO, string.Format("the packet is not be send"));
                                HandleException(SocketState.Disconnecting);
                            }
                        } while (sendBufferSize < sendLength);
                    }
                    else
                    {
                        WriteFiles.WritFile.Log(LogerType.DEBUG, string.Format("socket state: {0} ", this.m_SocketState));
                    }
                }
                catch (Exception e)
                {
                    WriteFiles.WritFile.Log(e);
                    HandleException(SocketState.Disconnecting);
                }
            }
        }

        protected void HandleException(SocketState status)
        {
            m_SocketState = status;
            ChangeStatus(status);
        }
    }
}