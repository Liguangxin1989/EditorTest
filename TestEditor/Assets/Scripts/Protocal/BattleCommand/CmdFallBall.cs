using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using System.Collections.Generic;

namespace BubbleCouple
{
    public sealed class CmdFallBall : BattleCommand
    {
        public int Reason
        {
            get
            {
                if (CmdData != null && CmdData.FallBall != null)
                {
                    return CmdData.FallBall.Reason;
                }

                return 0;
            }
        }

        public List<int> BallIds
        {
            get
            {
                if (CmdData != null && CmdData.FallBall != null)
                {
                    return CmdData.FallBall.BallIds;
                }

                return new List<int>();
            }
        }

        public int CollisionBallId
        {
            get
            {
                if (CmdData != null && CmdData.FallBall != null)
                {
                    return CmdData.FallBall.CollisionBallId;
                }

                return 0;
            }
        }

        private CmdFallBall() : base(EBattleCommandType.CmdFallBall)
        {

        }

        private CmdFallBall(StructBattleCommand cmd) : this()
        {
            CmdData = cmd;
        }

        public CmdFallBall(int side, int reason, List<int> ballIds, int collisionBallId = -1) : this()
        {
            CmdData = new StructBattleCommand();
            CmdData.Side = (sbyte)side;
            CmdData.Type = (int)Type;
            CmdData.FallBall = new FallBall();
            CmdData.FallBall.Reason = (sbyte)reason;
            CmdData.FallBall.BallIds = ballIds;
            if (reason == 0)
            {
                CmdData.FallBall.CollisionBallId = collisionBallId;
            }
        }
    }
}
