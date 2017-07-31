using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Common;
using NetWrapper.Network;
using NetWrapper;

namespace BubbleCouple
{
    public class NetworkMgr : Singleton<NetworkMgr>, IPeerListener
    {
        private TPeer m_peer;
        private bool m_isConnected = false;

        private IMessageHandler m_handler = null;

        public bool IsConnected
        {
            get { return m_isConnected; }
        }

        private NetworkMgr()
        {
            m_peer = new TPeer(this, false);
        }

        public void SetMessageHandler(IMessageHandler handler)
        {
            m_handler = handler;
        }

        public void ClearAll()
        {
        }

        public void Connect(string ip, int port)
        {
            m_peer.Connect(ip, port);
        }

        public void DisConnect()
        {
            m_peer.Disconnect();
        }

        public void ShutDown()
        {
            m_peer.ShutDown();
            m_peer = null;
        }

        public void Update(float fTime)
        {
            AsynTcpClient.Update();
        }

        public void Send(ProtoBase proto)
        {
            if (m_isConnected == false)
            {
                return;
            }

            StreamBuffer sb = proto.GetStream();
            m_peer.Send(sb);
        }

        #region IPeerListener
        public void OnConnected()
        {
            Debug.Log("OnConnect");
            m_isConnected = true;
            if (m_handler != null)
            {
                m_handler.OnConnect();
            }
        }

        public void OnDisconnected()
        {
            Debug.Log("OnDisconnect");
            m_isConnected = false;
            if (m_handler != null)
            {
                m_handler.OnDisconnect();
            }
        }

        public void OnPktReceive(StreamBuffer streamBuffer)
        {
            ProtoBase proto = ProtoFactory.ConstructThriftProto(streamBuffer);
            if (proto != null)
            {
                if (proto.MessageID == MessageId.SC_ClockSync)
                {
                    ServerTimeHelper.Instance.HandleSyncClockMessage(proto as ProtoSCClockSync);
                    return;
                }
                if (!proto.IsRoomMessage())
                {
                    Debug.Log("OnPktReceive: " + proto.ToString());
                    // 不包含ID字段，是与服务器通讯的消息，直接处理
                    if (m_handler != null)
                    {
                        m_handler.HandleMessage(proto);
                    }
                    return;
                }
            }
        }

        #endregion
    }
}
