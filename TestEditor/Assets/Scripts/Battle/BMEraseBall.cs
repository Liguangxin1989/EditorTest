using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace BubbleCouple
{
    public sealed class BMEraseBall : BattleMissionBase
    {
        private CmdEraseBall m_cmd;
        private int m_missionFinishNum;

        public BMEraseBall(EBattleCommandType type, BattleField battleField) : base(type, battleField)
        {
            
        }
        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            if(m_isOver)
            {
                return;
            }

            bool haveMissionBall = false;
            bool haveNormalBall = false;

            m_missionFinishNum = 0;
            for (int i = 0; i != m_cmd.BallIds.Count; ++ i)
            {
                int ballId = m_cmd.BallIds[i];
                if (m_balls.ContainsKey(ballId))
                {
                    if (m_balls.GetBallType(ballId) == ELevelBallType.Crown)
                    {
                        ++m_missionFinishNum;
                        haveMissionBall = true;
                    }
                    else
                    {
                        haveNormalBall = true;
                    }
                    GameObject.Destroy(m_balls.GetValue(ballId));
                    m_balls.RemoveKey(ballId);
                }
            }
            m_battleField.UpDataMissionProgress(m_missionFinishNum);

            if (haveNormalBall)
            {
                AudioMgr.PlayAudio(204);
            }

            if (haveMissionBall)
            {
                AudioMgr.PlayAudio(206);
            }

            m_isOver = true;
        }

        public override void SetCommand(BattleCommand cmd)
        {
            base.SetCommand(cmd);
            m_cmd = cmd as CmdEraseBall;

            m_isOver = false;
        }
    }
}