using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

namespace BubbleCouple
{
    public class ServerTimeHelper : Singleton<ServerTimeHelper>
    {
        private long m_timeDeviation = 0;

        public long ServerTime
        {
            get
            {
                return LocalTime + m_timeDeviation;
            }
        }

        public long LocalTime
        {
            get
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return (long)(DateTime.Now - startTime).TotalMilliseconds;
            }
        }

        private Action m_syncClockCallback = null;
        private long m_clientTime;
        private long m_serverTime;
        private long m_st1;
        private long m_minDelta;
        private int m_syncCount;

        private ServerTimeHelper()
        {

        }

        public void SyncClock(Action callback)
        {
            m_syncClockCallback = callback;
            m_syncCount = 0;
            SendSyncClock(0, 0, 0);
        }

        private void SendSyncClock(int step, long clientTime, long serverTime)
        {
            ProtoCSClockSync proto = new ProtoCSClockSync(step, clientTime, serverTime);
            NetworkMgr.Instance.Send(proto);
        }

        public void HandleSyncClockMessage(ProtoSCClockSync proto)
        {
            //Debug.Log("HandleSyncClockMessage: " + proto.ToString());
            if (proto.Step == 0)
            {
                m_st1 = proto.ServerTime;
                SendSyncClock(1, LocalTime, proto.ServerTime);
            }
            else
            {
                m_syncCount++;

                long cDelta = LocalTime - proto.ClientTime;
                long sDelta = proto.Delta;
                long minDelta = Math.Abs(cDelta - sDelta);

                if (m_syncCount == 1)
                {
                    m_minDelta = minDelta;
                    m_clientTime = proto.ClientTime;
                    m_serverTime = m_st1 + sDelta / 2;
                }
                else
                {
                    if (minDelta < m_minDelta)
                    {
                        m_minDelta = minDelta;
                        m_clientTime = proto.ClientTime;
                        m_serverTime = m_st1 + sDelta / 2;
                    }
                }

                if (m_syncCount >= 10)
                {
                    m_timeDeviation = m_serverTime - m_clientTime;
                    Debug.Log("m_timeDeviation = " + m_timeDeviation);
                    if (m_syncClockCallback != null)
                    {
                        m_syncClockCallback();
                    }
                }
                else
                {
                    SendSyncClock(0, 0, 0);
                }
            }
        }
    }
}
