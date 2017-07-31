using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class CmdFireBallResponse : BattleCommand
    {
        public int BallType
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return CmdData.FireBallResponse.Type;
                }

                return -1;
            }
        }

        public Vector2 Dir
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return new Vector2((float)CmdData.FireBallResponse.Dir.X, (float)CmdData.FireBallResponse.Dir.Y);
                }

                return Vector2.zero;
            }
        }

        public Vector2 CollisionPoint
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return new Vector2((float)CmdData.FireBallResponse.CollisionPoint.X, (float)CmdData.FireBallResponse.CollisionPoint.Y);
                }

                return Vector2.zero;
            }
        }

        public GridPosition DestGrid
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return new GridPosition(CmdData.FireBallResponse.DestGrid.Col, CmdData.FireBallResponse.DestGrid.Row);
                }

                return new GridPosition(0, 0);
            }
        }

        public int BallId
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return CmdData.FireBallResponse.BallId;
                }

                return -1;
            }
        }

        public float Speed
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return (float)CmdData.FireBallResponse.Speed;
                }

                return 0;
            }
        }

        public int Result
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return CmdData.FireBallResponse.Result;
                }

                return 0;
            }
        }

        public Vector2 StartPosition
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return new Vector2((float)CmdData.FireBallResponse.StartPosition.X, (float)CmdData.FireBallResponse.StartPosition.Y);
                }

                return Vector2.zero;
            }
        }

        public bool IsFlyOut
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return CmdData.FireBallResponse.Result == 2;
                }

                return false;
            }
        }

        public bool IsAddToScene
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return CmdData.FireBallResponse.Result == 1;
                }

                return false;
            }
        }

        public bool IsErase
        {
            get
            {
                if (CmdData != null && CmdData.FireBallResponse != null)
                {
                    return CmdData.FireBallResponse.Result == 0;
                }

                return false;
            }
        }

        private CmdFireBallResponse() : base(EBattleCommandType.CmdFireBallResponse)
        {

        }

        private CmdFireBallResponse(StructBattleCommand cmd) : this()
        {
            CmdData = cmd;
        }

        public CmdFireBallResponse(int side, int type, Vector2 startPosition, Vector2 dir, Vector2 collisionPoint, GridPosition destGrid, int ballId, float speed, int result) : this()
        {
            CmdData = new StructBattleCommand();
            CmdData.Side = (sbyte)side;
            CmdData.Type = (int)Type;
            CmdData.FireBallResponse = new FireBallResponse();
            CmdData.FireBallResponse.Type = type;
            CmdData.FireBallResponse.StartPosition = new Vector2D();
            CmdData.FireBallResponse.StartPosition.X = startPosition.x;
            CmdData.FireBallResponse.StartPosition.Y = startPosition.y;
            CmdData.FireBallResponse.Dir = new Vector2D();
            CmdData.FireBallResponse.Dir.X = dir.x;
            CmdData.FireBallResponse.Dir.Y = dir.y;
            CmdData.FireBallResponse.CollisionPoint = new Vector2D();
            CmdData.FireBallResponse.CollisionPoint.X = collisionPoint.x;
            CmdData.FireBallResponse.CollisionPoint.Y = collisionPoint.y;
            CmdData.FireBallResponse.DestGrid = new GridCoord();
            CmdData.FireBallResponse.DestGrid.Col = destGrid.col;
            CmdData.FireBallResponse.DestGrid.Row = destGrid.row;
            CmdData.FireBallResponse.BallId = ballId;
            CmdData.FireBallResponse.Speed = speed;
            CmdData.FireBallResponse.Result = (sbyte)result;
        }
    }
}
