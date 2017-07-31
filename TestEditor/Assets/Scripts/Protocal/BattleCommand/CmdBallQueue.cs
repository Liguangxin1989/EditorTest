using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using System.Collections.Generic;

namespace BubbleCouple
{
    public sealed class CmdBallQueue : BattleCommand
    {
        public List<int> AddBallQueue
        {
            get
            {
                if (CmdData != null && CmdData.BallQueue != null)
                {
                    return CmdData.BallQueue.Queue;
                }

                return null;
            }
        }

        public int AddBallCount
        {
            get
            {
                if (CmdData != null && CmdData.BallQueue != null)
                {
                    return CmdData.BallQueue.Queue.Count;
                }

                return 0;
            }
        }

        private CmdBallQueue() : base(EBattleCommandType.CmdBallQueue)
        {

        }

        private CmdBallQueue(StructBattleCommand cmd) : this()
        {
            CmdData = cmd;
        }

        public CmdBallQueue(int side, List<int> queue) : this()
        {
            CmdData = new StructBattleCommand();
            CmdData.Side = (sbyte)side;
            CmdData.Type = (int)Type;
            CmdData.BallQueue = new BallQueue();
            CmdData.BallQueue.Queue = queue;
        }
    }
}
