using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class SelectLevelBridge : UIBridge
    {
        private SelectLevel SubGame
        {
            get { return m_subGame as SelectLevel; }
        }

        private SelectLevelUI UI
        {
            get { return m_ui as SelectLevelUI; }
        }

        public SelectLevelBridge(SelectLevel subGame) : base (subGame, new SelectLevelUI())
        {

        }

        public void StartGame()
        {
            SubGame.StartGame();
        }

        public void SetSelectLevel(int levelNum)
        {
            SubGame.SetSelectLevel(levelNum);
        }

    }
}
