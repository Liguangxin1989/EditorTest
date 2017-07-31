using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using System.Collections.Generic;

namespace BubbleCouple
{
    public sealed class CmdExchangeBall : BattleCommand
    {
        private CmdExchangeBall() : base(EBattleCommandType.CmdExchangeBall)
        {

        }

        private CmdExchangeBall(StructBattleCommand cmd) : this()
        {
            CmdData = cmd;
        }

        public CmdExchangeBall(int side) : this()
        {
            CmdData = new StructBattleCommand();
            CmdData.Side = (sbyte)side;
            CmdData.Type = (int)Type;
        }
    }
}
