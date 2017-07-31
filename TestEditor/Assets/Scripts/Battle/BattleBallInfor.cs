using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace BubbleCouple
{
    public class BattleBallInfor : KeyDictionary<int, GameObject>
    {
        private Dictionary<int, ELevelBallType> m_ballsTypeDic;
        public BattleBallInfor() : base()
        {
            m_ballsTypeDic = new Dictionary<int, ELevelBallType>();
        }

        public override void RemoveKey(int key)
        {
            base.RemoveKey(key);
            m_ballsTypeDic.Remove(key);
        }

        public void AddBall(int id, GameObject ball, ELevelBallType type)
        {
            base.Add(id, ball);
            if (!m_ballsTypeDic.ContainsKey(id))
            {
                m_ballsTypeDic.Add(id, type);
            }
        }

        public override void Clear()
        {
            base.Clear();
            m_ballsTypeDic.Clear();
        }

        public ELevelBallType GetBallType(int id)
        {
            if (!m_ballsTypeDic.ContainsKey(id))
            {
                return ELevelBallType.None;
            }

            return m_ballsTypeDic[id];
        }
    }
}