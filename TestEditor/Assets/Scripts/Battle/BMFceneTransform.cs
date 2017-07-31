using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public sealed class BMFceneTransform : BattleMissionBase
{
    private CmdSceneTransform m_cmd;
    private float m_fallSpeed;
    private float m_timer;

    public BMFceneTransform(EBattleCommandType type, BattleField battleField) : base(type, battleField)
    {

    }
    public override void Update(float fDeltaTime)
    {
        base.Update(fDeltaTime);

        if(m_isOver)
        {
            return;
        }

        m_battleField.SetSceneTransform(m_cmd.Translation);
        m_isOver = true;
    }

    public override void SetCommand(BattleCommand cmd)
    {
        base.SetCommand(cmd);
        m_cmd = cmd as CmdSceneTransform;
       
        m_isOver = false;
    }
}
}
