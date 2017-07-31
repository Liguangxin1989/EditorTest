using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace BubbleCouple
{
    public sealed class BMFallBall : BattleMissionBase
    {
        private CmdFallBall m_cmd;
        private float m_fallSpeed;
        private float m_timer;
        private readonly float m_fallTime;
        private int m_missionFinishNum;

        public BMFallBall(EBattleCommandType type, BattleField battleField) : base(type, battleField)
        {
            m_fallTime = ConfigHelper.GetSysConfig(ESysConfig.BubbleFallTime)*0.001f;
        }
        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            if (m_isOver)
            {
                return;
            }
            for (int i = 0; i != m_cmd.BallIds.Count; ++ i)
            {
                int ballId = m_cmd.BallIds[i];
                if (m_balls.ContainsKey(ballId))
                {
                    Vector2 pos = m_balls.GetValue(ballId).transform.localPosition;
                    pos.y += m_fallSpeed*fDeltaTime;
                    m_balls.GetValue(ballId).transform.localPosition = pos;
                }
            }
            m_timer += fDeltaTime;
            if (m_timer >= m_fallTime)
            {
                for(int i = 0; i != m_cmd.BallIds.Count; ++i)
                {
                    int ballId = m_cmd.BallIds[i];
                    if(m_balls.ContainsKey(ballId))
                    {
                        GameObject.Destroy(m_balls.GetValue(ballId));
                        m_balls.RemoveKey(ballId);
                    }
                }
                m_battleField.UpDataMissionProgress(m_missionFinishNum);
                m_isOver = true;
            }
        }

        public override void SetCommand(BattleCommand cmd)
        {
            base.SetCommand(cmd);
            m_cmd = cmd as CmdFallBall;
            m_fallSpeed = GlobalData.m_ballFallSpeed;
            m_missionFinishNum = 0;
            for(int i = 0; i != m_cmd.BallIds.Count; ++i)
            {
                int ballId = m_cmd.BallIds[i];
                if(m_balls.ContainsKey(ballId))
                {
                    m_balls.GetValue(ballId).GetComponent<Collider2D>().enabled = false;
                    if (m_balls.GetBallType(ballId) == ELevelBallType.Crown)
                    {
                        ++ m_missionFinishNum;
                    }
                }
            }
            if (GameCore.Instance.MySide == 0)
            {
                m_fallSpeed = -m_fallSpeed;
            }
            m_timer = 0f;
            m_isOver = false;
        }
    }
}