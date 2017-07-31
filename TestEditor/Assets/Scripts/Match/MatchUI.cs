using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace BubbleCouple
{
    public class MatchUI : UIBase
    {
        private MatchBridge Bridge
        {
            get { return m_bridge as MatchBridge; }
        }

        private Button m_btnEnter;
        private InputField m_roomId;
        private Text m_error;

        public MatchUI()
        {

        }

        public override void Init(UIBridge bridge)
        {
            base.Init(bridge);

            GameObject canvas = GameObject.Find("Canvas");
            m_btnEnter = ObjectEX.FindComponentInChildren<Button>(canvas, "BtnJoinRoom");
            m_roomId = ObjectEX.FindComponentInChildren<InputField>(canvas, "InputField");
            Button btnOffline = ObjectEX.FindComponentInChildren<Button>(canvas, "BtnOffline");
            m_error = ObjectEX.FindComponentInChildren<Text>(canvas, "Error");

            m_roomId.text = GlobalConfig.Instance.SimulatorRoomId;
            m_error.text = "";
            m_btnEnter.interactable = true;
            m_btnEnter.onClick.AddListener(OnEnterClick);
            btnOffline.onClick.AddListener(OnOfflineClick);
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

        public void OnJoinRoom(bool bSuccess, string error)
        {
            if (!bSuccess)
            {
                m_btnEnter.interactable = true;
                m_error.text = error;
            }
        }

        public void OnEnterClick()
        {
            if (m_roomId.text != "")
            {
                AudioMgr.PlayAudio(100);
                Bridge.JoinRoom(m_roomId.text);
                m_btnEnter.interactable = false;
            }
        }

        public void OnOfflineClick()
        {
            Bridge.OfflineMode();
        }
    }
}
