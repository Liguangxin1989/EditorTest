using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class LoginBridge : UIBridge
    {
        private Login SubGame
        {
            get { return m_subGame as Login; }
        }

        private LoginUI UI
        {
            get { return m_ui as LoginUI; }
        }

        public LoginBridge(Login subGame) : base (subGame, new LoginUI())
        {

        }

        public override void Init()
        {
            base.Init();
        }

        public override void Destroy()
        {
            base.Destroy();
        }


        public void OfflineMode()
        {
            SubGame.OfflineMode();
        }

        public void OnlineMode()
        {
            SubGame.OnlineMode();
        }

        public void ResetServer()
        {
            SubGame.ResetServer();
        }
    }
}
