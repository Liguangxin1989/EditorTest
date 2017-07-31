using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;


namespace BubbleCouple
{
    public class BattleLocalCal
    {
        private LineRenderer m_lr;
        private RaycastHit2D m_hit;
        private BattleBallInfor m_balls;
        private Battle m_battle;
        private GameObject m_battleFieldObj;

        private int m_lastBallId;

        public bool Init(Battle battle, GameObject battleFieldObj)
        {
            m_battle = battle;
            m_battleFieldObj = battleFieldObj;
            GameObject lro = new GameObject("LineRender");
            m_hit = new RaycastHit2D();
            m_lr = lro.AddComponent<LineRenderer>();
            m_lr.startWidth = 0.096f;
            m_lr.endWidth = 0.096f;
            m_lastBallId = 11111;
            return true;
        }

        public void InitLevel(LevelMapData data, BattleBallInfor balls)
        {
            m_balls = balls;
        }

        public void ClearLineRender()
        {
            m_lr.positionCount = 0;
        }

        public void CalFireBall(int side, ELevelBallType type, Vector2 startPos, Vector2 fingerPos, float lineLength = 0f, bool isPresent = false)
        {
            ClearLineRender();
            float drawLineLength = lineLength;
           
            Vector2 dir = (fingerPos - startPos).normalized;
            Vector2 startPoint = startPos;

            Vector2 normal = dir;
            if(normal.y == 0)
            {
                Debug.Log("CallFireBall Y shoud != 0!");
                return;
            }

            float flyDistance = 0;
            bool isCalOver = false;
            bool hasBouncedBack = false;
            do
            {
                Vector2 endPos = RayCast(startPoint, normal);
                if (m_hit.collider == null)
                {
                    flyDistance += Vector2.Distance(startPoint, endPos);
                    if (isPresent)
                    {
                        drawLineLength -= DrawLine(startPoint, endPos, normal, drawLineLength);
                    }

                    if (endPos.y >= GlobalData.m_windowHight/2 || endPos.y <= (-GlobalData.m_windowHight/2) || hasBouncedBack)
                    {
                        //打空了
                        if (!isPresent)
                        {
                            SendMessage(side, type, startPos, dir, endPos, true, flyDistance);
                        }
                        return;
                    }

                    startPoint = endPos;
                    normal.x = -normal.x;
                    hasBouncedBack = true;
                    
                }
                else
                {
                    //打中了
                    flyDistance += Vector2.Distance(startPoint, m_hit.point);
                    if (isPresent)
                    {
                        drawLineLength -= DrawLine(startPoint, m_hit.point, normal, drawLineLength);
                    }
                    else
                    {
                        SendMessage(side, type, startPos, dir, m_hit.point, false, flyDistance);
                    }

                    isCalOver = true;
                }

            } while (!isCalOver);
        }

        private void SendMessage(int side, ELevelBallType type, Vector2 startPos, Vector2 dir, Vector2 hitPoint, bool isFlyout, float flyDistance)
        {
            Vector2 point = m_battleFieldObj.transform.InverseTransformPoint(hitPoint);
            Vector2 cellPoint = point;
            float speed = flyDistance / ConfigHelper.GetBubbleFlyDuration();
            cellPoint.y = -cellPoint.y;
            if (GameCore.Instance.IsOfflineMode)
            {
                CmdFireBallResponse cmd = new CmdFireBallResponse(0, (int)type, startPos, dir, point, CellManager.GetCellID(cellPoint), m_lastBallId, speed, 0);
                ++m_lastBallId;
                m_battle.HandleBattleCommand(cmd);
            }
            else
            {
                ProtoCSBattleCommand proto = new ProtoCSBattleCommand(new CmdFireBall(side, (int)type, startPos, dir, point, CellManager.GetCellID(cellPoint), isFlyout, speed));
                NetworkMgr.Instance.Send(proto);

            }
        }

        private float DrawLine(Vector2 startPos, Vector2 endPos, Vector2 normal, float lineLength)        {            if (lineLength <= 0)            {                return 0f;            }            float length = Vector2.Distance(startPos, endPos);            if (length > lineLength)            {                endPos = startPos + (normal * lineLength);                length = lineLength;            }            var numPositions = m_lr.positionCount;            if (numPositions >= 2)            {                m_lr.positionCount += 1;                m_lr.SetPosition(m_lr.positionCount - 1, endPos);            }            else            {                m_lr.positionCount += 2;                m_lr.SetPosition(m_lr.positionCount - 2, startPos);                m_lr.SetPosition(m_lr.positionCount - 1, endPos);            }            return length;        }

        private Vector2 RayCast(Vector2 startPos, Vector2 normal)
        {
            float length;
            if(normal.x == 0)
            {
                length = GlobalData.m_windowHight + 1f;
            }
            else
            {
                length = CalLineLength(startPos, normal);
            }

            Vector2 endPos = length * normal + startPos;
            m_hit = Physics2D.Linecast(startPos, endPos);
            return endPos;
        }

        private float CalLineLength(Vector2 startPos, Vector2 lineNormal)
        {
            float result = 1f;
            float horLength = Mathf.Abs((lineNormal.x > 0 
                ? (GlobalData.m_windowWidth/2 - startPos.x - GlobalData.m_ballRadius)
                : (startPos.x - (-GlobalData.m_windowWidth/2 + GlobalData.m_ballRadius))) / lineNormal.x);
            float colLength = Mathf.Abs((lineNormal.y > 0
                ? (GlobalData.m_windowHight/2 - startPos.y)
                : (startPos.y - (-GlobalData.m_windowHight/2)))/lineNormal.y) + 1f;

            return Mathf.Min(horLength, colLength);
        }

        public void Destory()
        {
            GameObject.Destroy(m_lr.gameObject);
        }
    }
}
