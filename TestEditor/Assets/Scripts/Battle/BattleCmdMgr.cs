using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace BubbleCouple
{
    public class BattleCmdMgr
    {
        private BattleField m_battleField;
        private List<Queue<BattleMissionBase>> m_missionList;
        private Dictionary<EBattleCommandType, Queue<BattleMissionBase>> m_usableMissionDic;
        private Queue<BattleMissionBase> m_lastQueue;
        public BattleBallInfor m_balls;
        public GameObject m_battleFieldObj;
        protected GameObject[] m_waitBalls;
        protected ELevelBallType[] m_waitBallsType;
        private bool m_isIdle;

        public bool Init(BattleField battleField)
        {
            m_battleField = battleField;
            m_missionList = new List<Queue<BattleMissionBase>>();
            m_usableMissionDic = new Dictionary<EBattleCommandType, Queue<BattleMissionBase>>();
            foreach(EBattleCommandType item in Enum.GetValues(typeof(EBattleCommandType)))
            {
                m_usableMissionDic.Add(item, new Queue<BattleMissionBase>());
            }

            m_battleFieldObj = m_battleField.BattleFieldObj;
            m_balls = m_battleField.Balls;
            m_waitBalls = m_battleField.WaitBalls;
            m_waitBallsType = m_battleField.WaitBallsType;
            m_isIdle = true;
            return true;
        }

        public void Update(float fDeltaTime)
        {
            if (m_isIdle)
            {
                return;
            }

            List<Queue<BattleMissionBase>> rmQueueList = new List<Queue<BattleMissionBase>>();
            for (int i = 0; i < m_missionList.Count; ++ i)
            {
                BattleMissionBase currMission = m_missionList[i].Peek();
                currMission.Update(fDeltaTime);
                if (currMission.IsOver)
                {
                    m_usableMissionDic[currMission.Type].Enqueue(currMission);
                    m_missionList[i].Dequeue();
                    if (m_missionList[i].Count <= 0)
                    {
                        rmQueueList.Add(m_missionList[i]);
                    }
                }
            }

            for (int i = 0; i < rmQueueList.Count; ++ i)
            {
                m_missionList.Remove(rmQueueList[i]);
            }
            rmQueueList.Clear();

            if (m_missionList.Count == 0)
            {
                m_isIdle = true;
            }

            if (m_lastQueue != null && m_lastQueue.Count == 0)
            {
                m_lastQueue = null;
            }
        }

        public void AddCommand(BattleCommand cmd)
        {
            BattleMissionBase newMission = CreateMission(cmd);
            if (newMission == null)
            {
                return;
            }
            newMission.SetCommand(cmd);


            if (m_lastQueue != null)
            {
                if (cmd.Type == EBattleCommandType.CmdFireBallResponse || cmd.Type == EBattleCommandType.CmdExchangeBall)
                {
                    AddNewMissionList(cmd, newMission);
                }
                else
                {
                    m_lastQueue.Enqueue(newMission);
                }
            }
            else
            {
                AddNewMissionList(cmd, newMission);
            }

            m_isIdle = false;
        }

        public void AddNewMissionList(BattleCommand cmd, BattleMissionBase newMission)
        {
            m_lastQueue = new Queue<BattleMissionBase>();
            m_lastQueue.Enqueue(newMission);
            m_missionList.Add(m_lastQueue);
        }

        private BattleMissionBase CreateMission(BattleCommand cmd)
        {
            BattleMissionBase newMission;
            if(m_usableMissionDic[cmd.Type].Count > 0)
            {
                newMission = m_usableMissionDic[cmd.Type].Dequeue();
            }
            else
            {
                newMission = BattleMissionBase.CreateBattleMission(cmd.Type, m_battleField);
                if (newMission == null)
                {
                    Debug.Log("Can't Create BattleMission type : " + cmd.Type);
                }
                if (newMission != null)
                {
                    newMission.Init();
                }
            }

            return newMission;
        }

        public void Destory()
        {
            
        }
    }
}
