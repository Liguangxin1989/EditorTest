using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class MatchBridge : UIBridge
    {
        private Match SubGame
        {
            get { return m_subGame as Match; }
        }

        private MatchUI UI
        {
            get { return m_ui as MatchUI; }
        }

        public MatchBridge(Match subGame) : base (subGame, new MatchUI())
        {

        }

        public override void Init()
        {
            base.Init();

            SubGame.OnJoinRoom += OnJoinRoom;
        }

        public override void Destroy()
        {
            SubGame.OnJoinRoom -= OnJoinRoom;
            base.Destroy();
        }

        public void JoinRoom(string roomId)
        {
            SubGame.JoinRoom(roomId);
        }

        public void OfflineMode()
        {
            SubGame.OfflineMode();
        }

        public void OnJoinRoom(bool bSuccess, string error)
        {
            UI.OnJoinRoom(bSuccess, error);
        }


    }
}
