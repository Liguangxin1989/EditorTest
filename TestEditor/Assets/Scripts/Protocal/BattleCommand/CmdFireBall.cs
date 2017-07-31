using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class CmdFireBall : BattleCommand
    {
        public int BallType
        {
            get
            {
                if (CmdData != null && CmdData.FireBall != null)
                {
                    return CmdData.FireBall.Type;
                }

                return -1;
            }
        }

        public Vector2 Dir
        {
            get
            {
                if (CmdData != null && CmdData.FireBall != null)
                {
                    return new Vector2((float)CmdData.FireBall.Dir.X, (float)CmdData.FireBall.Dir.Y);
                }

                return Vector2.zero;
            }
        }

        public Vector2 CollisionPoint
        {
            get
            {
                if (CmdData != null && CmdData.FireBall != null)
                {
                    return new Vector2((float)CmdData.FireBall.CollisionPoint.X, (float)CmdData.FireBall.CollisionPoint.Y);
                }

                return Vector2.zero;
            }
        }

        public GridPosition DestGrid
        {
            get
            {
                if (CmdData != null && CmdData.FireBall != null)
                {
                    return new GridPosition(CmdData.FireBall.DestGrid.Col, CmdData.FireBall.DestGrid.Row);
                }

                return new GridPosition(0, 0);
            }
        }

        public Vector2 StartPosition
        {
            get
            {
                if (CmdData != null && CmdData.FireBall != null)
                {
                    return new Vector2((float)CmdData.FireBall.StartPosition.X, (float)CmdData.FireBall.StartPosition.Y);
                }

                return Vector2.zero;
            }
        }

        public bool IsFlyOut
        {
            get
            {
                if (CmdData != null && CmdData.FireBall != null)
                {
                    return CmdData.FireBall.IsFlyout;
                }

                return false;
            }
        }

        private CmdFireBall() : base(EBattleCommandType.CmdFireBall)
        {

        }

        private CmdFireBall(StructBattleCommand cmd) : this()
        {
            CmdData = cmd;
        }

        public CmdFireBall(int side, int type, Vector2 startPosition, Vector2 dir, Vector2 collisionPoint, GridPosition destGrid, bool isFlyout, float speed) : this()
        {
            CmdData = new StructBattleCommand();
            CmdData.Side = (sbyte)side;
            CmdData.Type = (int)Type;
            CmdData.FireBall = new FireBall();
            CmdData.FireBall.Type = type;
            CmdData.FireBall.StartPosition = new Vector2D();
            CmdData.FireBall.StartPosition.X = startPosition.x;
            CmdData.FireBall.StartPosition.Y = startPosition.y;
            CmdData.FireBall.Dir = new Vector2D();
            CmdData.FireBall.Dir.X = dir.x;
            CmdData.FireBall.Dir.Y = dir.y;
            CmdData.FireBall.CollisionPoint = new Vector2D();
            CmdData.FireBall.CollisionPoint.X = collisionPoint.x;
            CmdData.FireBall.CollisionPoint.Y = collisionPoint.y;
            CmdData.FireBall.DestGrid = new GridCoord();
            CmdData.FireBall.DestGrid.Col = destGrid.col;
            CmdData.FireBall.DestGrid.Row = destGrid.row;
            CmdData.FireBall.IsFlyout = isFlyout;
            CmdData.FireBall.Speed = speed;
        }
    }
}
