using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace BubbleCouple
{
    public class SelectLevelUI : UIBase
    {
        private SelectLevelBridge Bridge
        {
            get { return m_bridge as SelectLevelBridge; }
        }

        private GameObject m_levelGroup;
        private GameObject m_leveInformation;
        private Text m_informationText;

        public SelectLevelUI()
        {

        }

        public override void Init(UIBridge bridge)
        {
            base.Init(bridge);
            m_levelGroup = GameObject.Find("Level");
            m_leveInformation = GameObject.Find("LevelInformation");

            if (m_levelGroup == null || m_leveInformation == null)
            {
                Debug.LogError("can't find Game Object : Level or LevelInformation!");
                return;
            }
            m_leveInformation.SetActive(false);
            GameObject btn = Resources.Load<GameObject>("Prefabs/LevelBtn");
            if (btn == null)
            {
                Debug.LogError("can't find Game Object : Prefabs/LevelBtn!");
            }

            int totalLevelCount = LevelConfig.Instance.TotalLevelCount;
            for (int i = 1; i <= totalLevelCount; ++i)
            {
                GameObject newBtn = GameObject.Instantiate(btn);
                newBtn.name = i.ToString();
                newBtn.transform.SetParent(m_levelGroup.transform);
                Button button = newBtn.GetComponent<Button>();
                button.onClick.AddListener( () => OnSelectClick(newBtn.name) );
                button.interactable = GameCore.Instance.IsMaster;
            }

            GameObject infor = ObjectEX.GetGameObjectByName(m_leveInformation, "Information");
            m_informationText = ObjectEX.FindComponentInChildren<Text>(m_leveInformation, "Information");
            Button startBtn = ObjectEX.FindComponentInChildren<Button>(m_leveInformation, "StartGame");
            Button CancelBtn = ObjectEX.FindComponentInChildren<Button>(m_leveInformation, "Cancel");

            startBtn.onClick.AddListener(OnStartGameClick);
            CancelBtn.onClick.AddListener(OnCancelClick);
        }

        public void OnSelectClick(string senderName)
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            m_leveInformation.SetActive(true);
            Bridge.SetSelectLevel(int.Parse(senderName));
        }

        public void OnStartGameClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            Bridge.StartGame();
        }

        public void OnCancelClick()
        {
            AudioMgr.PlayAudio(100, Guid.Empty);
            m_leveInformation.SetActive(false);
        }
    }
}
