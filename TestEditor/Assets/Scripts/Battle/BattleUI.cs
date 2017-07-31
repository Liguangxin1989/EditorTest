using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace BubbleCouple
{
    public class BattleUI : UIBase
    {
        private BattleBridge Bridge
        {
            get { return m_bridge as BattleBridge; }
        }

        private GameObject m_winPanel;
        private GameObject m_losePanel;
        private Button m_nextBtn;
        private Text m_noBallTip;
        private RectTransform m_leftMask;
        private RectTransform m_rightMask;
        private float m_noBallTipTimer;
        private GameObject m_canvas;
        private Text m_myBallNumText;
        private Text m_mateBallNumText;
        private Text m_missionInfor;
        //private MutiRect m_mask;

        /*
        private Text m_myBallNum;
        private Text m_mateBallNum;
        */
        public BattleUI()
        {

        }

        public override void Init(UIBridge bridge)
        {
            base.Init(bridge);
            GameObject middleCanvas = GameObject.Find("MiddleCanvas");
            m_winPanel = ObjectEX.GetGameObjectByName(middleCanvas, "PanelWin");
            m_losePanel = ObjectEX.GetGameObjectByName(middleCanvas, "PanelLose");

            GameObject winPanel = ObjectEX.GetGameObjectByName(m_winPanel, "Panel");
            GameObject losePanel = ObjectEX.GetGameObjectByName(m_losePanel, "Panel");

            Button btnWinClose = ObjectEX.FindComponentInChildren<Button>(winPanel, "ButtonBack");
            Button btnWinNext = ObjectEX.FindComponentInChildren<Button>(winPanel, "ButtonNext");
            Button btnLoseClose = ObjectEX.FindComponentInChildren<Button>(losePanel, "ButtonBack");
            Button btnLoseRestart = ObjectEX.FindComponentInChildren<Button>(losePanel, "ButtonRestart");
            m_nextBtn = btnWinNext;

            m_canvas = GameObject.Find("Canvas");
            //m_mask = ObjectEX.GetGameObjectByName(canvas, "Mask").GetComponent<MutiRect>();
            m_leftMask = ObjectEX.GetGameObjectByName(m_canvas, "LeftMask").GetComponent<RectTransform>();
            m_rightMask = ObjectEX.GetGameObjectByName(m_canvas, "RightMask").GetComponent<RectTransform>();
            m_noBallTip = ObjectEX.FindComponentInChildren<Text>(m_canvas, "NoBallTip");
            m_myBallNumText = ObjectEX.FindComponentInChildren<Text>(m_canvas, "MyBallNum");
            m_mateBallNumText = ObjectEX.FindComponentInChildren<Text>(m_canvas, "MateBallNum");
            m_missionInfor = ObjectEX.FindComponentInChildren<Text>(m_canvas, "MissionInfor");
            m_noBallTip.gameObject.SetActive(false);
            m_noBallTipTimer = -1f;
            /*
            m_myBallNum = ObjectEX.FindComponentInChildren<Text>(canvas, "MyBallNum");
            m_mateBallNum = ObjectEX.FindComponentInChildren<Text>(canvas, "MateBallNum");
            */
            btnWinClose.onClick.AddListener(OnBackClick);
            btnWinNext.onClick.AddListener(OnNextClick);
            btnLoseClose.onClick.AddListener(OnBackClick);
            btnLoseRestart.onClick.AddListener(OnRestartClick);

            btnWinClose.interactable = GameCore.Instance.IsMaster;
            btnWinNext.interactable = GameCore.Instance.IsMaster;
            btnLoseClose.interactable = GameCore.Instance.IsMaster;
            btnLoseRestart.interactable = GameCore.Instance.IsMaster;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (m_noBallTipTimer >= 0f)
            {
                m_noBallTip.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, m_noBallTipTimer);
                m_noBallTipTimer -= deltaTime;
                if (m_noBallTipTimer < 0f)
                {
                    m_noBallTip.gameObject.SetActive(false);
                }
            }
        }

        public override void OnAdaptResolution()
        {
            base.OnAdaptResolution();
            //m_mask.AddRect(new Rect((1f - width) / 2f, 0, width, 1f));
            float width = Screen.height/16f*9f;
            RectTransform canvasRect = m_canvas.GetComponent<RectTransform>();
            float canvasMultiple = canvasRect.sizeDelta.x / Screen.width;
            float maskWidth = (Screen.width - width)/2f*canvasMultiple;

            if (maskWidth > 0)
            {
                m_leftMask.sizeDelta = new Vector2(maskWidth, 0);
                m_rightMask.sizeDelta = new Vector2(maskWidth, 0);
                Vector2 anchoredPos = m_missionInfor.GetComponent<RectTransform>().anchoredPosition;
                anchoredPos.x = maskWidth;
                m_missionInfor.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
                anchoredPos = m_myBallNumText.GetComponent<RectTransform>().anchoredPosition;
                anchoredPos.x = maskWidth;
                m_myBallNumText.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
                anchoredPos = m_mateBallNumText.GetComponent<RectTransform>().anchoredPosition;
                anchoredPos.x = - maskWidth;
                m_mateBallNumText.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
                m_leftMask.gameObject.SetActive(true);
                m_rightMask.gameObject.SetActive(true);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public void OnSetBallNum(int myBallNum, int mateBallNum)
        {
            m_myBallNumText.text = myBallNum.ToString();
            m_mateBallNumText.text = mateBallNum.ToString();
        }

        public void OnSetMissionProgress(int missionFinishNum, int missionNum)
        {
            m_missionInfor.text = "目标: " + missionFinishNum + " / " + missionNum;
        }

        public void OnShowNoBallTip(Vector2 pos)
        {
            m_noBallTip.gameObject.SetActive(true);
            m_noBallTip.transform.position = pos;
            m_noBallTipTimer = 1f;
        }

        public void OnRestartBattle()
        {
            m_winPanel.SetActive(false);
            m_losePanel.SetActive(false);
            m_myBallNumText.text = "";
            m_mateBallNumText.text = "";
            m_missionInfor.text = "";
        }

        public void OnStartBattle()
        {
        }

        public void OnEndBattle(bool isWin)
        {
            if (isWin)
            {
                m_winPanel.SetActive(true);
                if (GameCore.Instance.IsMaster)
                {
                    m_nextBtn.interactable = GameCore.Instance.m_globalData.CurrLevel < LevelConfig.Instance.TotalLevelCount;
                }
                AudioMgr.PlayAudio(20);
            }
            else
            {
                m_losePanel.SetActive(true);
                AudioMgr.PlayAudio(21);
            }
        }

        public void OnBackClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            Bridge.LeaveBattle();
        }

        public void OnNextClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            Bridge.NextLevel();
            m_winPanel.SetActive(false);
        }

        public void OnRestartClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            Bridge.RestartBattle();
            m_losePanel.SetActive(false);
        }

        public void OnRotateBackGround()
        {
            GameObject backGround = GameObject.Find("BackGround");
            if (backGround != null)
            {
                var pos = backGround.transform.position;
                pos.z = -pos.z;

                var rot = Quaternion.Euler(180f, 0, 0);

                backGround.transform.position = pos;
                backGround.transform.rotation = rot;
            }
        }
    }
}
