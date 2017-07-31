using System;
using NetWrapper.Network.Tool;

namespace NetWrapper.Network
{
    public class ReliableOrderServerPeer : ReliableOrderPeer
    {
        public ReliableOrderServerPeer(IGameSession iSession, IPeerListener peerListener)
            : base(iSession, peerListener)
        {
        }

        public int GetPeerID()
        {
            return m_peerSession.GetSessionID();
        }

        public void StartServerPeer(IPeerListener peerListener)
        {
            SetPeerListener(peerListener);
            ((ServerSession)m_peerSession).SessionReceive(this);
        }

        public override void Disconnect()
        {
            m_peerSession.DisConnect();
        }

        protected override void OnDisconnected()
        {
            m_peerListener.OnDisconnected();
            AsynTcpServer.Instance.RemoveSession(m_peerSession.GetSessionID());
        }
    }
}