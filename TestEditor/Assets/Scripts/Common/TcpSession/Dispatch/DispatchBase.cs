//  Dispatch.cs
//  Nilsen
//  2015-04-09
using System.Collections.Generic;

namespace NetWrapper.Network
{
    /// <summary>
    /// 调度类
    /// </summary>
    public abstract class DispatchBase
    {
        protected ISessionListener m_sessionListener;
        protected ISession m_cSession;
        private Queue<StreamBuffer> m_cReceiveQueue = new Queue<StreamBuffer>();
        private Queue<StreamBuffer> m_cDispatchQueue = new Queue<StreamBuffer>();

        public DispatchBase(ISessionListener sessionListener, ISession session)
        {
            this.m_sessionListener = sessionListener;
            this.m_cReceiveQueue = new Queue<StreamBuffer>();
            this.m_cDispatchQueue = new Queue<StreamBuffer>();
            this.m_cSession = session;
        }

        /// <summary>
        /// 连接事件
        /// </summary>
        public virtual void OnConnect()
        {
            m_sessionListener.OnConnect();
        }
        public void SetSessionListener(ISessionListener sessionListener)
        {
            m_sessionListener = sessionListener;
        }
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public virtual void OnDisconnect()
        {
            m_sessionListener.OnDisconnect();
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="pb"></param>
        public virtual void AckPacket(StreamBuffer sb)
        {
            lock (this.m_cReceiveQueue)
            {
                this.m_cReceiveQueue.Enqueue(sb);
            }
        }

        /// <summary>
        /// 逻辑更新
        /// </summary>
        /// <returns></returns>
        public virtual void Update()
        {
            if(this.m_cReceiveQueue.Count > 0)
            {
                lock (this.m_cReceiveQueue)
                {
                    Queue<StreamBuffer>.Enumerator iter = m_cReceiveQueue.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        m_cDispatchQueue.Enqueue(iter.Current);
                    }
                    this.m_cReceiveQueue.Clear();
                }
            }

            for (int i = 0; i < 16; i++)
            {
                if(this.m_cDispatchQueue.Count > 0)
                {
                    StreamBuffer sb = this.m_cDispatchQueue.Dequeue();
                    m_sessionListener.OnPktReceive(sb);
                }
                else
                {
                    break;
                }
            }

            m_sessionListener.OnDispatch();
        }
    }
}