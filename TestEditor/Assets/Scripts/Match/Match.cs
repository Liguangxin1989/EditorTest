using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{
    public class Match : SubGame
    {
        public delegate void JoinRoomHandler(bool bSuccess, string error);
        public event JoinRoomHandler OnJoinRoom;

        public Match() : base(ESubGame.Match)
        {

        }

        public override void Init()
        {
            base.Init();
            SceneHelper.LoadSceneAsync("Match", OnSceneLoaded);
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
            return new MatchBridge(this);
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            if (m_bSceneLoaded == false)
            {
                return;
            }
        }

        public override void HandleMessage(ProtoBase proto)
        {
            base.HandleMessage(proto);

            switch (proto.MessageID)
            {
                case MessageId.SC_JoinGame:
                    {
                        ProtoSCJoinGame joinGame = proto as ProtoSCJoinGame;
                        if (joinGame.Result == JoinGameResult.JGR_Success)
                        {
                            OnJoinRoom(true, "");
                        }
                        else if (joinGame.Result == JoinGameResult.JGR_UserFull)
                        {
                            OnJoinRoom(false, "房间已满，无法进入");
                        }
                        else if (joinGame.Result == JoinGameResult.JGR_BattleFull)
                        {
                            OnJoinRoom(false, "服务器爆满，无法创建房间");
                        }
                        else if (joinGame.Result == JoinGameResult.JGR_AlreadyInBattle)
                        {
                            OnJoinRoom(false, "已经在房间中");
                        }
                        break;
                    }
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

        public void JoinRoom(string roomId)
        {
            ProtoCSJoinGame proto = new ProtoCSJoinGame(roomId);
            NetworkMgr.Instance.Send(proto);
        }

        public void OfflineMode()
        {
            GameCore.Instance.IsOfflineMode = true;
            NetworkMgr.Instance.DisConnect();
            GameCore.Instance.SwitchToSubgame(ESubGame.Battle);
        }
    }
}
