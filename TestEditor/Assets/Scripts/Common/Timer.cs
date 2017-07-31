using System;
using UnityEngine;
using System.Collections.Generic;

namespace Common
{
    public class Timer
    {

        private static IDGenerater m_idGenerater = new IDGenerater();

        private List<DataItem> m_timerItems;
        private List<DataItem> m_timerItemsToAdd;
        private List<DataItem> m_timerItemsToRemove;
        private List<uint> m_timerIDsToKill;

        public Timer()
        {
            m_timerItems = new List<DataItem>();
            m_timerItemsToAdd = new List<DataItem>(5);
            m_timerItemsToRemove = new List<DataItem>(5);
            m_timerIDsToKill = new List<uint>(5);
        }


        public uint StartTimer(Action action, float delay)
        {
            var id = m_idGenerater.GetID();
            var item = new DataItem(id, action, delay);
            m_timerItemsToAdd.Add(item);

            return id;
        }

        public void StopTimer(uint timerID)
        {
            m_timerIDsToKill.Add(timerID);
        }

        public void Update(float fDeltaTime)
        {
            // add new item.
            if(m_timerItemsToAdd.Count > 0)
            {
                for(int i = 0; i < m_timerItemsToAdd.Count; ++i)
                {
                    m_timerItems.Add(m_timerItemsToAdd[i]);
                }

                m_timerItems.Sort();
                m_timerItemsToAdd.Clear();
            }

            // update and execute item.
            {
                for (int i = 0; i < m_timerItems.Count; ++i)
                {
                    var item = m_timerItems[i];
                    var id = item.id;

                    if (m_timerIDsToKill.Contains(id))
                    {
                        m_timerItemsToRemove.Add(item);
                    }
                    else
                    {
                        item.Update(fDeltaTime);
                        if (item.TimeToExecute())
                        {
                            item.Execute();
                            m_timerItemsToRemove.Add(item);
                        }
                    }
                }

                m_timerIDsToKill.Clear();
            }


            // remove item.
            if(m_timerItemsToRemove.Count > 0)
            {
                for(int i = 0; i < m_timerItemsToRemove.Count; ++i)
                {
                    m_timerItems.Remove(m_timerItemsToRemove[i]);
                }

                m_timerItemsToRemove.Clear();
            }
        }



        private class DataItem : IComparable
        {
            public uint id;
            public Action callback;
            public float timeToDelay;

            public DataItem(uint _id, Action _callback, float _timeToDelay)
            {
                id = _id;
                callback = _callback;
                timeToDelay = _timeToDelay;
            }

            public void Update(float fdeltaTime)
            {
                timeToDelay -= fdeltaTime;
            }

            public int CompareTo(object obj)
            {
                var item = obj as DataItem;
                return timeToDelay.CompareTo(item.timeToDelay);
            }

            public bool TimeToExecute()
            {
                return timeToDelay <= 0;
            }

            public void Execute()
            {
                if (callback != null)
                    callback();
            }
        }

        private class IDGenerater
        {
            private uint m_currentTimerID = 0;

            public IDGenerater()
            {
                m_currentTimerID = 0;
            }

            public uint GetID()
            {
                if (m_currentTimerID == uint.MaxValue)
                    m_currentTimerID = 0;

                return m_currentTimerID++;
            }
        }
    }
}