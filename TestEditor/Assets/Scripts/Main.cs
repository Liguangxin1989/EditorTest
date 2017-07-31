using UnityEngine;
using System.Collections;
using Common;

namespace BubbleCouple
{
    public class Main : MonoBehaviour
    {
        private static string m_mainName = "MainGO";
        private static GameObject m_maingo = null;
        private GameCore m_gameMgr = null;

        public static void BootGame()
        {
            if (m_maingo == null && !GameObject.Find(m_mainName))
            {
                Main mScript = ObjectEX.CreatGOWithBehaviour<Main>(m_mainName);
                m_maingo = mScript.gameObject;

                var monoHelper = ObjectEX.AddSingleComponent<MonoHelper>(m_maingo);

                Application.runInBackground = true;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
        }

        void Awake()
        {
            m_gameMgr = GameCore.Instance;

            if (!m_gameMgr.Init())
            {
                Debug.LogError("GameMgr Init Error !");
                Application.Quit();
            }
        }

        void Update()
        {
            m_gameMgr.UpdateGame(Time.deltaTime);
        }

        void FixedUpdate()
        {
            m_gameMgr.FixedUpdateGame(Time.fixedDeltaTime);
        }
        void OnApplicationQuit()
        {
        }

        void OnApplicationPause(bool status)
        {
            m_gameMgr.OnAppPause(status);
        }

        void OnApplicationFocus(bool status)
        {
#if UNITY_EDITOR
            // 编辑器环境下用Focus模拟Pause
            //m_gameMgr.OnAppPause(!status);
#endif
        }

        void OnDestroy()
        {
            m_gameMgr.OnGameEnd();
        }
    }
}


