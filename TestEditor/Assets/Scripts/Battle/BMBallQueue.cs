using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public sealed class BMBallQueue : BattleMissionBase
    {
        private CmdBallQueue m_cmd;

        public BMBallQueue(EBattleCommandType type, BattleField battleField) : base(type, battleField)
        {

        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            if (IsOver)
            {
                return;
            }

            if (m_cmd.AddBallCount > 0)
            {
                for (int i = 0; i < m_cmd.AddBallQueue.Count; ++ i)
                {
                    m_battleField.m_ballReserve[Side].Enqueue((ELevelBallType)m_cmd.AddBallQueue[i]);
                }

                m_battleField.CheckBallDisplay(m_cmd.Side);
            }

            m_isOver = true;
        }

        public override void SetCommand(BattleCommand cmd)
        {
            base.SetCommand(cmd);
            m_cmd = cmd as CmdBallQueue;
            m_isOver = false;
        }
    }
}
