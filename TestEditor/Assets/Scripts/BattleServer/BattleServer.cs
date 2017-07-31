using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

namespace BubbleCouple
{
    public class BattleServer : Singleton<BattleServer>
    {
        private Queue<BattleCommand> m_requestCommands = new Queue<BattleCommand>();

        private Action<BattleCommand> m_commandHandler;

        private ServerBattleField m_battleField;

        private BattleServer()
        {
            m_battleField = new ServerBattleField();
        }

        public void StartServer(int levelId, Action<BattleCommand> handler)
        {
            m_commandHandler = handler;
            m_requestCommands.Clear();
            m_battleField.InitLevel(levelId);
        }

        public void StopServer()
        {
            m_commandHandler = null;
            m_requestCommands.Clear();
            m_battleField.Clear();
        }

        public void Update(float fDeltaTime)
        {
            while (m_requestCommands.Count > 0)
            {
                BattleCommand command = m_requestCommands.Dequeue();
                ExecuteCommand(command);
            }
        }

        public void AddRequest(BattleCommand command)
        {
            m_requestCommands.Enqueue(command);
        }

        public void SendResponse(BattleCommand command)
        {
            if (m_commandHandler != null)
            {
                m_commandHandler(command);
            }
        }

        private void ExecuteCommand(BattleCommand command)
        {
            switch (command.Type)
            {
                case EBattleCommandType.CmdFireBall:
                    {
                        m_battleField.HandleFireBall(command as CmdFireBall);
                        break;
                    }
            }
        }
    }
}
