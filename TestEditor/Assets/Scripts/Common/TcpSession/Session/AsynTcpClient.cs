//  ClientSessionManager.cs
//  Nilsen
//  2015-04-08

using System;
using System.Collections.Generic;
using NetWrapper.Network;
using NetWrapper.Network.Tool;

namespace NetWrapper
{
    /// <summary>
    /// 客户端会话接口管理
    /// </summary> 
	public class AsynTcpClient : ISessionManager
    {
        private static AsynTcpClient m_sInstance = new AsynTcpClient();
        internal static AsynTcpClient Instance
        {
            get
            {
                return m_sInstance;
            }
        }

        private AsynTcpClient()
            : base()
        {
        }

        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="dispatchFactory"></param>
        /// <returns></returns>
        internal ClientSession InstanceSession(ISessionListener sessionListener)
        {
            ClientSession gs = new ClientSession(GetCurrSessionID(), sessionListener);
            this.m_addVecSession[gs.GetSessionID()] = gs;
            return gs;
        }

        /// <summary>
        /// 逻辑更新
        /// </summary>
        protected override void DoUpdate()
        {
            if (m_addVecSession.Count > 0)
            {
                lock (m_addVecSession)
                {
                    Dictionary<int, IGameSession>.Enumerator iterAdd = m_addVecSession.GetEnumerator();
                    while (iterAdd.MoveNext())
                    {
                        m_vecSession.Add(iterAdd.Current.Key, iterAdd.Current.Value);
                    }

                    m_addVecSession.Clear();
                }
            }

            Dictionary<int, IGameSession>.Enumerator iter = m_vecSession.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.Update();
            }

            if (this.m_removeIDList.Count > 0)
            {
                lock(m_removeIDList)
                {
                    int listLength = this.m_removeIDList.Count;
                    for (int index = 0; index < listLength; index++)
                    {
                        if (this.m_vecSession.ContainsKey(this.m_removeIDList[index]))
                        {
                            this.m_vecSession.Remove(this.m_removeIDList[index]);
                        }
                    }

                    this.m_removeIDList.Clear();
                }
            }
        }

        public static void Update()
        {
            Instance.DoUpdate();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sessionID"></param>
        internal void RemoveSession(int sessionID)
        {
            DoRemoveSession(sessionID);
        }
    }
}