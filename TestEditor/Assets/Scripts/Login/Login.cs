using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{
    
    public class Login : SubGame
    {
        private float m_checkConnectTimer = 0.0f;
        private static readonly float s_checkConnectCD = 5.0f;

        public Login() : base(ESubGame.Login)
        {

        }

        #region override members
        public override void Init()
        {
            base.Init();

            SceneHelper.LoadSceneAsync("Login", OnSceneLoaded);
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override void HandleMessage(ProtoBase proto)
        {
            base.HandleMessage(proto);
            switch (proto.MessageID)
            {
                case MessageId.SC_EnterGame:
                    {
                        ProtoSCEnterGame enterBattle = proto as ProtoSCEnterGame;
                        if (enterBattle.IsSuccess)
                        {
                            GameCore.Instance.MySide = enterBattle.Side;
                        }
                        break;
                    }
                case MessageId.SC_MatchSuccess:
                    {
                        GameCore.Instance.SwitchToSubgame(ESubGame.SelectLevel);
                        break;
                    }
            }
        }


        protected override UIBridge CreateUIBridge()
        {
            return new LoginBridge(this);
        }

        public override void OnSceneLoaded(bool bLoaded)
        {
            base.OnSceneLoaded(bLoaded);

            CheckConnect();
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            if (m_bSceneLoaded == false)
            {
                return;
            }

            m_checkConnectTimer += fDeltaTime;
            if (m_checkConnectTimer > s_checkConnectCD)
            {
                CheckConnect();
            }
        }

#endregion

        private void CheckConnect()
        {
            m_checkConnectTimer = 0.0f;
            if (NetworkMgr.Instance.IsConnected == false)
            {
                Debug.Log("ConnectServer: " + GlobalConfig.Instance.GameServerAddress);
                NetworkMgr.Instance.Connect(GlobalConfig.Instance.GameServerAddress, GlobalConfig.Instance.GameServerPort);
            }
            else
            {
                GameCore.Instance.SwitchToSubgame(ESubGame.Match);
            }
        }

        public void OfflineMode()
        {
            GameCore.Instance.IsOfflineMode = true;
            NetworkMgr.Instance.DisConnect();
            GameCore.Instance.SwitchToSubgame(ESubGame.SelectLevel);
        }

        public void OnlineMode()
        {
            ProtoCSEnterGame proto = new ProtoCSEnterGame();
            NetworkMgr.Instance.Send(proto);
        }

        public void ResetServer()
        {
            ProtoCSResetServer proto = new ProtoCSResetServer();
            NetworkMgr.Instance.Send(proto);
        }
    }
}
