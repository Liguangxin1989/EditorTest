using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{
    public class SelectLevel : SubGame
    {
        public SelectLevel() : base(ESubGame.SelectLevel)
        {

        }

        public override void Init()
        {
            base.Init();
            SceneHelper.LoadSceneAsync("SelectLevel", OnSceneLoaded);
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override void OnSceneLoaded(bool bLoaded)
        {
            base.OnSceneLoaded(bLoaded);
            AudioMgr.PlayAudio(1);
        }

        protected override UIBridge CreateUIBridge()
        {
            return new SelectLevelBridge(this);
        }

        public void StartGame()
        {
            if (GameCore.Instance.IsMaster)
            {
                ProtoCSStartLevel proto = new ProtoCSStartLevel(GameCore.Instance.m_globalData.CurrLevel);
                NetworkMgr.Instance.Send(proto);
            }
        }

        public void SetSelectLevel(int levelNum)
        {
            GameCore.Instance.m_globalData.CurrLevel = levelNum;
        }
    }
}
