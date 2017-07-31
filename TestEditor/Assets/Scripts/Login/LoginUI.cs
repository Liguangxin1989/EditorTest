using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace BubbleCouple
{
    public class LoginUI : UIBase
    {
        private LoginBridge Bridge
        {
            get { return m_bridge as LoginBridge; }
        }

        public LoginUI()
        {

        }

        public override void Init(UIBridge bridge)
        {
            base.Init(bridge);

            GameObject canvas = GameObject.Find("Canvas");
            Button btnOffline = ObjectEX.FindComponentInChildren<Button>(canvas, "BtnOffline");
            Button btnOnline = ObjectEX.FindComponentInChildren<Button>(canvas, "BtnOnline");
            Button btnReset = ObjectEX.FindComponentInChildren<Button>(canvas, "BtnReset");

            btnOffline.onClick.AddListener(OnOfflineClick);
            btnOnline.onClick.AddListener(OnOnlineClick);
            btnReset.onClick.AddListener(OnResetClick);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void OnAdaptResolution()
        {
            base.OnAdaptResolution();
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public void OnOfflineClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            Bridge.OfflineMode();
        }

        public void OnOnlineClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            Bridge.OnlineMode();
        }

        public void OnResetClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            Bridge.ResetServer();
        }
    }
}
