using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace BubbleCouple
{
    public sealed class BMAddBall : BattleMissionBase
    {
        private GameObject m_ball;
        private CmdFireBallResponse m_cmd;
        private Vector2 m_dir;
        private bool m_hasBouncedBack;
        private Vector2 m_collisionPoint;

        public BMAddBall(EBattleCommandType type, BattleField battleField) : base(type, battleField)
        {
            
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            if(m_isOver)
            {
                return;
            }

            Vector2 lpos = m_ball.transform.localPosition;
            lpos += m_dir*m_cmd.Speed*fDeltaTime;

            if (CheckPoint(m_ball.transform.localPosition, lpos))
            {
                if (m_cmd.Result == 0 || m_cmd.Result == 2)
                {
                    GameObject.Destroy(m_ball);
                }
                else
                {
                    m_ball.transform.localPosition = CellManager.GetCellPosition(m_cmd.DestGrid);
                    m_balls.AddBall(m_cmd.BallId, m_ball, (ELevelBallType)m_cmd.Type);
                    m_ball.GetComponent<Collider2D>().enabled = true;
                    AudioMgr.PlayAudio(205);
                }
                m_ball = null;
                m_isOver = true;
            }
            else
            {
                m_ball.transform.localPosition = lpos;
                Vector2 pos = m_ball.transform.position;
                if ((pos.x + GlobalData.m_ballRadius >= GlobalData.m_windowWidth/2 ||
                    pos.x - GlobalData.m_ballRadius <= -GlobalData.m_windowWidth/2))
                {
                    if (m_hasBouncedBack && (m_cmd.Result == 0 || m_cmd.Result == 2))
                    {
                        GameObject.Destroy(m_ball);
                        m_ball = null;
                        m_isOver = true;
                        return;
                    }
                    m_dir.x = -m_dir.x;
                    m_hasBouncedBack = true;
                    AudioMgr.PlayAudio(202);
                }
            }
        }

        private bool CheckPoint(Vector2 currPos, Vector2 nextPos)
        {
            bool yLess = currPos.y > m_collisionPoint.y;
            bool xLess = nextPos.y > m_collisionPoint.y;
            if (yLess != xLess)
            {
                return true;
            }

            return false;
        }

        public override void SetCommand(BattleCommand cmd)
        {
            base.SetCommand(cmd);
            m_cmd = cmd as CmdFireBallResponse;
            if (m_cmd.Result == 3)
            {
                return;
            }
            m_ball = BattleResource.Instance.GetBallResource((ELevelBallType)m_cmd.BallType);
            if(m_ball != null)
            {
                m_ball = GameObject.Instantiate(m_ball) as GameObject;
                m_ball.transform.position = m_cmd.StartPosition;
                m_ball.transform.SetParent(m_battleFieldObj.transform);
                m_ball.GetComponent<Collider2D>().enabled = false;
            }
            else
            {
                Debug.LogError("Can't Get Ball Resourcem, type:" + m_cmd.BallType);
                return;
            }
            if (m_cmd.Result == 0 || m_cmd.Result == 2)
            {
                m_collisionPoint = new Vector2(0, GlobalData.m_windowHight/2);
                if (m_cmd.Side == 0)
                {
                    m_collisionPoint.y = -m_collisionPoint.y;
                }
                m_collisionPoint = m_battleFieldObj.transform.InverseTransformPoint(m_collisionPoint);
            }
            else
            {
                m_collisionPoint = m_cmd.CollisionPoint;
            }
            m_dir = m_cmd.Dir;
            m_battleField.SetBallReady(m_cmd.Side);
            AudioMgr.PlayAudio(203);
            m_hasBouncedBack = false;
            m_isOver = false;
        }
    }
}