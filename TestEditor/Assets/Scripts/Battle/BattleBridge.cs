using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class BattleBridge : UIBridge
    {
        private Battle SubGame
        {
            get { return m_subGame as Battle; }
        }

        private BattleUI UI
        {
            get { return m_ui as BattleUI; }
        }

        public BattleBridge(Battle subGame) : base (subGame, new BattleUI())
        {

        }

        public override void Init()
        {
            base.Init();

            SubGame.OnRestartBattle += OnRestartBattle;
            SubGame.OnStartBattle += OnStartBattle;
            SubGame.OnEndBattle += OnEndBattle;
            SubGame.OnShowNoBallTip += OnShowNoBallTip;
            SubGame.OnSetBallNum += OnSetBallNum;
            SubGame.OnSetMissionProgress += OnOnSetMissionProgress;
            SubGame.OnRotateBackGround += OnRotateBackGround;
        }

        public override void Destroy()
        {
            SubGame.OnRestartBattle -= OnRestartBattle;
            SubGame.OnStartBattle -= OnStartBattle;
            SubGame.OnEndBattle -= OnEndBattle;
            SubGame.OnShowNoBallTip -= OnShowNoBallTip;
            SubGame.OnSetBallNum -= OnSetBallNum;
            SubGame.OnSetMissionProgress -= OnOnSetMissionProgress;
            base.Destroy();
        }

        public void OnRestartBattle()
        {
            UI.OnRestartBattle();
        }

        public void OnStartBattle()
        {
            UI.OnStartBattle();
        }

        public void OnEndBattle(bool isWin)
        {
            UI.OnEndBattle(isWin);
        }
        public void OnSetBallNum(int myBallNum, int mateBallNum)
        {
            UI.OnSetBallNum(myBallNum, mateBallNum);
        }

        public void OnOnSetMissionProgress(int finishNum, int missionNum)
        {
            UI.OnSetMissionProgress(finishNum, missionNum);
        }

        public void OnShowNoBallTip(Vector2 pos)
        {
            UI.OnShowNoBallTip(pos);
        }

        public void OnRotateBackGround()
        {
            UI.OnRotateBackGround();
        }

        public void LeaveBattle()
        {
            SubGame.LeaveBattle();
        }

        public void NextLevel()
        {
            SubGame.NextLevel();
        }

        public void RestartBattle()
        {
            SubGame.RestartBattle();
        }
    }
}
