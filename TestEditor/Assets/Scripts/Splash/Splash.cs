using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using Common;
using HedgehogTeam.EasyTouch;

namespace BubbleCouple
{
    public sealed class Splash : SubGame
    {
        private bool m_bFinished = false;

        public Splash():base(ESubGame.SplashScreen)
        {

        }

        public override void Init()
        {
            LoadConfig();
            GameCore.Instance.InitEasyTouch();

            m_bFinished = true;
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            if (m_bFinished)
            {
                AudioMgr.PlayAudio(1, Guid.Empty);
                if (Input.GetMouseButtonDown(0))
                {

                    GameCore.Instance.SwitchToSubgame(ESubGame.Login);
                }
            }
        }


        public override void Destroy()
        {

        }

        public override void OnPause(bool status)
        {
            base.OnPause(status);
        }

        public override void OnTap(Gesture gesture)
        {
            base.OnTap(gesture);
        }

        private void LoadConfig()
        {
            if (!GameConfigMgr.Instance.Init())
            {
                Debug.LogError("Config Manager Init Error!");
            }
        }

    } // SplashScreen
} // PUzzlTD


