using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HedgehogTeam.EasyTouch;

namespace BubbleCouple
{
    public class GameCore : Common.Singleton<GameCore>, IMessageHandler
    {
        private ESubGame m_curSubGameType = ESubGame.None;
        private ESubGame m_newSubGameType = ESubGame.None;

        private GameResourceMgr m_resMgr = null;
        private SubGame m_curSubGame = null;

        private bool m_isOfflineMode = false;

        public GlobalData m_globalData;
        private bool m_gamePause;

        private int m_mySide;

        public bool IsOfflineMode
        {
            get { return m_isOfflineMode; }
            set { m_isOfflineMode = value; }
        }

        public bool IsMaster
        {
            get
            {
                return MySide == 0;
            }
        }

        public int MySide
        {
            get
            {
                if (IsOfflineMode)
                {
                    return 0;
                }
                return m_mySide;
            }
            set
            {
                m_mySide = value;
            }
        }

        private GameCore()
        {
            m_resMgr = new GameResourceMgr();
            m_globalData = new GlobalData();
            m_gamePause = false;
    }

        public bool Init()
        {
            NetworkMgr.Instance.SetMessageHandler(this);
            SwitchToSubgame(ESubGame.SplashScreen);

            TestScene.SetEnable(false);

            return true;
        }

        public void SwitchToSubgame(ESubGame subgame)
        {
            m_newSubGameType = subgame;
        }

        // FPS temp code
        private float m_fpsTimer;
        private int m_fpsCount;
        public void UpdateGame(float fDeltaTime)
        {
            
            // FPS temp code
            {
                ++m_fpsCount;
                m_fpsTimer += Time.deltaTime;
                if (m_fpsTimer >= 0.5f)
                {
                    TestScene.ShowLabel("FPS", (m_fpsCount / m_fpsTimer).ToString("f2"));
                    m_fpsCount = 0;
                    m_fpsTimer = 0f;
                }
            }

            // switch subgame
            if (m_newSubGameType != m_curSubGameType
                && m_newSubGameType != ESubGame.None)
            {
                if (m_curSubGame != null)
                {
                    m_curSubGame.Destroy();
                    m_curSubGame = null;
                }

                m_curSubGame = GetSubGameInstance(m_newSubGameType);
                m_curSubGame.Init();
                m_curSubGameType = m_newSubGameType;
            }

            // update subgame
            if (m_curSubGame != null && !m_gamePause)
            {
                m_curSubGame.Update(fDeltaTime);
            }
            if (m_curSubGameType != ESubGame.SplashScreen)
            {
                NetworkMgr.Instance.Update(fDeltaTime);
                //EducationServer.Instance.Update(fDeltaTime);
                //AntiAddiction.Instance.Update(fDeltaTime);
            }
        }

        public void FixedUpdateGame(float fDeltaTime)
        {

        }

        public void OnGameEnd()
        {
            NetworkMgr.Instance.ShutDown();
        }

        public void OnAppFocus(bool status)
        {
            
        }

        public void OnAppPause(bool status)
        {

        }


        private SubGame GetSubGameInstance(ESubGame type)
        {
            SubGame subGame = null;

            switch(type)
            {
                case ESubGame.SplashScreen:
                    subGame = new Splash();
                    break;
                case ESubGame.Login:
                    subGame = new Login();
                    break;
                case ESubGame.Match:
                    subGame = new Match();
                    break;
                case ESubGame.Battle:
                    subGame = new Battle();
                    break;
                case ESubGame.SelectLevel:
                    subGame = new SelectLevel();
                    break;

                default:
                    Debug.LogError("Unsupported SubGame Type!");
                    break;
            }

            return subGame;
        }


        public void PauseGame(string reason)
        {

        }

        public void ResumeGame()
        {

        }


        #region EasyTouch
        private void OnTap(Gesture gesture)
        {
            if (m_curSubGame != null)
                m_curSubGame.OnTap(gesture);
        }

        private void OnDrag(Gesture gesture)
        {
            if(m_curSubGame != null)
                m_curSubGame.OnDrag(gesture);
        }

        private void OnDragStart(Gesture gesture)
        {
            if(m_curSubGame != null)
                m_curSubGame.OnDragStart(gesture);
        }

        private void OnDragEnd(Gesture gesture)
        {
            if (m_curSubGame != null)
                m_curSubGame.OnDragEnd(gesture);
        }

        private void OnTouchStart(Gesture gesture)
        {
            if(m_curSubGame != null)
                m_curSubGame.OnTouchStart(gesture);
        }

        private void OnTouchDown(Gesture gesture)
        {
            if(m_curSubGame != null)
                m_curSubGame.OnTouchDown(gesture);
        }
        private void OnTouchUp(Gesture gesture)
        {
            if(m_curSubGame != null)
                m_curSubGame.OnTouchUp(gesture);
        }

        public void InitEasyTouch()
        {
            GameObject.DontDestroyOnLoad(EasyTouch.instance.gameObject);

            EasyTouch.On_SimpleTap += this.OnTap;
            EasyTouch.On_Drag += this.OnDrag;
            EasyTouch.On_DragStart += this.OnDragStart;
            EasyTouch.On_DragEnd += this.OnDragEnd;
            EasyTouch.On_TouchStart += this.OnTouchStart;
            EasyTouch.On_TouchDown += this.OnTouchDown;
            EasyTouch.On_TouchUp += this.OnTouchUp;
        }
        #endregion

        public void ExitRoom(string reason = "", bool bShowDisconnect = false)
        {
            m_gamePause = false;

            GameCore.Instance.SwitchToSubgame(ESubGame.Login);
        }

        #region IMessageHandler

        public void OnConnect()
        {
            SwitchToSubgame(ESubGame.Match);
        }

        public void OnDisconnect()
        {
            ExitRoom();
        }

        public void HandleMessage(ProtoBase proto)
        {
            switch (proto.MessageID)
            {
                case MessageId.SC_StartLevel:
                    {
                        if (m_curSubGameType != ESubGame.Battle)
                        {
                            ProtoSCStartLevel startLevel = proto as ProtoSCStartLevel;
                            m_globalData.CurrLevel = startLevel.LevelId;
                            SwitchToSubgame(ESubGame.Battle);
                        }
                        break;
                    }
                case MessageId.SC_LeaveGame:
                    {
                        ExitRoom();
                        break;
                    }
                default:
                    break;
            }
            // update subgame
            if (m_curSubGame != null)
            {
                m_curSubGame.HandleMessage(proto);
            }
        }
        #endregion
    }
}
