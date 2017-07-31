using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace BubbleCouple
{
    public sealed class BMExChangeBall : BattleMissionBase
    {
        private CmdExchangeBall m_cmd;

        public BMExChangeBall(EBattleCommandType type, BattleField battleField) : base(type, battleField)
        {

        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            m_battleField.ExChangeBall(m_cmd.Side);
            AudioMgr.PlayAudio(101);
            m_isOver = true;
        }

        public override void SetCommand(BattleCommand cmd)
        {
            base.SetCommand(cmd);
            m_cmd = cmd as CmdExchangeBall;
            m_isOver = false;
        }
    }
}