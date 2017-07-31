using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using System.Collections.Generic;

namespace BubbleCouple
{
    public sealed class CmdEraseBall : BattleCommand
    {
        public int Reason
        {
            get
            {
                if (CmdData != null && CmdData.EraseBall != null)
                {
                    return CmdData.EraseBall.Reason;
                }

                return 0;
            }
        }

        public List<int> BallIds
        {
            get
            {
                if (CmdData != null && CmdData.EraseBall != null)
                {
                    return CmdData.EraseBall.BallIds;
                }

                return new List<int>();
            }
        }

        public int CollisionBallId
        {
            get
            {
                if (CmdData != null && CmdData.EraseBall != null)
                {
                    return CmdData.EraseBall.CollisionBallId;
                }

                return 0;
            }
        }

        private CmdEraseBall() : base(EBattleCommandType.CmdEraseBall)
        {

        }

        private CmdEraseBall(StructBattleCommand cmd) : this()
        {
            CmdData = cmd;
        }

        public CmdEraseBall(int side, int reason, List<int> ballIds, int collisionBallId = -1) : this()
        {
            CmdData = new StructBattleCommand();
            CmdData.Side = (sbyte)side;
            CmdData.Type = (int)Type;
            CmdData.EraseBall = new EraseBall();
            CmdData.EraseBall.Reason = (sbyte)reason;
            CmdData.EraseBall.BallIds = ballIds;
            if (reason == 0)
            {
                CmdData.EraseBall.CollisionBallId = collisionBallId;
            }
        }
    }
}
