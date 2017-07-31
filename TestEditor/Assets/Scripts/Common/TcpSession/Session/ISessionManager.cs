//  ISessionManager.cs
//  Nilsen
//  2015-04-08
using System;
using System.Collections.Generic;

namespace NetWrapper.Network
{
    // <summary>
    // 会话管理类
    // </summary>
    public abstract class ISessionManager
    {
        protected int m_currSessionID;
        protected Dictionary<int, IGameSession> m_vecSession; 
        protected Dictionary<int, IGameSession> m_addVecSession;
        protected List<int> m_removeIDList;

        public ISessionManager()
        {
            this.m_currSessionID = -1;
            this.m_vecSession = new Dictionary<int, IGameSession>();
            this.m_addVecSession = new Dictionary<int, IGameSession>();
            this.m_removeIDList = new List<int>();
        }

        protected abstract void DoUpdate();  //逻辑更新

        protected void DoRemoveSession(int sessionID)
        {
            if (!(this.m_vecSession.ContainsKey(sessionID) || this.m_addVecSession.ContainsKey(sessionID)))
            {
                return;
            }

            lock (m_removeIDList)
            {
                if(!m_removeIDList.Contains(sessionID))
                {
                    this.m_removeIDList.Add(sessionID);
                }
            }
        }

        protected int GetCurrSessionID()
        {
            this.m_currSessionID += 1;
            if (this.m_currSessionID >= Int32.MaxValue)
            {
                this.m_currSessionID = 0;
            }
            return this.m_currSessionID;
        }
    }
}