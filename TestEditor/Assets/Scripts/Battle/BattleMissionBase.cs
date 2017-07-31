using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace BubbleCouple
{
    public enum BallType
    {
        Red,
        Yellow,
        Blue,
        Green,
        Purple,

        BaseBallNum,

        Crown,
    }
    public class BattleMissionBase
    {
        protected BattleField m_battleField;
        protected bool m_isOver;
        protected GameObject m_battleFieldObj;
        protected BattleBallInfor m_balls;
        protected GameObject[] m_waitBalls;
        protected ELevelBallType[] m_waitBallsType;
        private EBattleCommandType m_cmdType;
        private int m_side;

        public BattleMissionBase(EBattleCommandType cmdType, BattleField battleField)
        {
            m_battleField = battleField;
            m_battleFieldObj = m_battleField.BattleFieldObj;
            m_balls = m_battleField.Balls;
            m_cmdType = cmdType;
            m_waitBalls = m_battleField.WaitBalls;
            m_waitBallsType = m_battleField.WaitBallsType;
        }

        public int Side
        {
            get { return m_side; }
        }

        public EBattleCommandType Type
        {
            get { return m_cmdType; }
        }

        public bool IsOver
        {
            get { return m_isOver; }
        }

        public virtual bool Init()
        {
            m_isOver = false;
            return true;
        }

        public virtual void Update(float fDeltaTime)
        {
            
        }

        public virtual void SetCommand(BattleCommand cmd)
        {
            m_side = cmd.Side;
        }

        public static BattleMissionBase CreateBattleMission(EBattleCommandType type, BattleField battleField)
        {
            BattleMissionBase bm = null;
            switch (type)
            {
                case EBattleCommandType.CmdFireBallResponse:
                    bm = new BMAddBall(type, battleField);
                    break;
                case EBattleCommandType.CmdEraseBall:
                    bm = new BMEraseBall(type, battleField);
                    break;
                case EBattleCommandType.CmdFallBall:
                    bm = new BMFallBall(type, battleField);
                    break;
                case EBattleCommandType.CmdExchangeBall:
                    bm = new BMExChangeBall(type, battleField);
                    break;
                case EBattleCommandType.CmdBallQueue:
                    bm = new BMBallQueue(type, battleField);
                    break;
                case EBattleCommandType.CmdSceneTransform:
                    bm = new BMFceneTransform(type, battleField);
                    break;
                default:
                    bm = null;
                    break;
            }

            return bm;
        }
    }
}
