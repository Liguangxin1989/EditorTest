//  ClientSession.cs
//  Nilsen
//  2015-04-09

using System;
using System.Net.Sockets;
using NetWrapper.Network.Tool;
using NetWrapper.Network;
using System.Threading;

namespace NetWrapper
{
    /// <summary>
    /// 网络会话类
    /// </summary>
    public class ClientSession : IGameSession
    {
        public ClientSession(int sessionID, ISessionListener sessionListener)
            : base(sessionID, sessionListener)
        {
        }
    }

    public class ServerSession : IGameSession
    {
        public ServerSession(int sessionID, ISessionListener sessionListener, Socket remoteSocket)
            : base(sessionID, sessionListener)
        {
            this.m_cSocket = remoteSocket;
            this.m_cSocket.NoDelay = true;
        }

        public void SessionReceive(ISessionListener sessionListener)
        {
            m_cDispatch.SetSessionListener(sessionListener);
            m_SocketState = SocketState.Connected;
            ChangeStatus(SocketState.Connected);

            m_receiveThread = new Thread(new ThreadStart(ReceiveLoop)) { Name = "receive thread", IsBackground = true };
            m_receiveThread.Start();
            m_sendThread = new Thread(new ThreadStart(SendLoop)) { Name = "send loop", IsBackground = true };
            m_sendThread.Start();
        }
    }
}