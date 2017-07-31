using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using HedgehogTeam.EasyTouch;


namespace BubbleCouple
{
    public class Battle : SubGame
    {
        public delegate void RestartBattleHandler();
        public delegate void StartBattleHandler();
        public delegate void RotateBackGround();
        public delegate void EndBattleHandler(bool isWin);
        public delegate void ShowNoBallTip(Vector2 pos);
        public delegate void SetBallNum(int myBallNum, int mateBallBum);
        public delegate void SetMissionProgress(int finishNum, int missionNum);

        public event RestartBattleHandler OnRestartBattle;
        public event StartBattleHandler OnStartBattle;
        public event EndBattleHandler OnEndBattle;
        public event ShowNoBallTip OnShowNoBallTip;
        public event SetBallNum OnSetBallNum;
        public event SetMissionProgress OnSetMissionProgress;
        public event RotateBackGround OnRotateBackGround;

        private bool m_isBeginFire = false;

        private enum EBattleState
        {
            None,
            SyncClock,
            Preparing,
            Ready,
            Fighting,
        }

        private EBattleState m_state = EBattleState.None;
        private long m_startFightTime = 0;
        private long m_startFightDelay = 2000;  // 开始战斗统一延迟
        private float m_viewSceneTime;
        private float m_viewSceneSpeed;
        private Vector2 m_viewSceneTarget;

        private BattleField m_battleField;
        private BattleLocalCal m_battleLocalCal;

        public Battle() : base(ESubGame.Battle)
        {

        }

        public override void Init()
        {
            base.Init();
            m_startFightDelay = ConfigHelper.GetSysConfig(ESysConfig.ViewSceneTime) + 500;
            m_viewSceneTime = (float)ConfigHelper.GetSysConfig(ESysConfig.ViewSceneTime) * 0.001f;
            SceneHelper.LoadSceneAsync("Battle", OnSceneLoaded);
        }

        public override void Destroy()
        {
            base.Destroy();
            m_battleField.Destroy();
            m_battleLocalCal.Destory();

            if (GameCore.Instance.IsOfflineMode)
            {
                BattleServer.Instance.StopServer();
            }
        }

        public override void OnSceneLoaded(bool bLoaded)
        {
            base.OnSceneLoaded(bLoaded);
            BattleResource.Instance.LoadResources();

            if (GameCore.Instance.IsOfflineMode)
            {
                BattleServer.Instance.StartServer(1, HandleBattleCommand);
            }

            if (GameCore.Instance.MySide == 0)
            {
                var pos = Camera.main.transform.position;
                pos.z = -pos.z;

                var rotX =  180f;
                var rot = Quaternion.Euler(rotX, 0f, 180f);

                Camera.main.transform.position = pos;
                Camera.main.transform.rotation = rot;
            }
            else
            {
                var rot = Quaternion.Euler(0f, 0f, 180f);
                Camera.main.transform.rotation = rot;
                OnRotateBackGround();
            }

            m_battleField = new BattleField(DoSetBallNum, DoSetMissionProgress);
            m_battleLocalCal = new BattleLocalCal();
            m_battleField.Init();
            m_battleLocalCal.Init(this, m_battleField.BattleFieldObj);

            Reset();
            SyncClock();
        }

        protected override UIBridge CreateUIBridge()
        {
            return new BattleBridge(this);
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            if (m_bSceneLoaded == false)
            {
                return;
            }

            if (m_state == EBattleState.Ready)
            {
                float x = m_battleField.BattleFieldObj.transform.position.x + m_viewSceneSpeed * fDeltaTime;
                if (x > m_viewSceneTarget.x)
                {
                    x = m_viewSceneTarget.x;
                }

                m_battleField.BattleFieldObj.transform.position = new Vector2(x, m_viewSceneTarget.y);

                if (m_startFightTime > 0 && ServerTimeHelper.Instance.ServerTime >= m_startFightTime)
                {
                    m_battleField.BattleFieldObj.transform.position = m_viewSceneTarget;
                    m_state = EBattleState.Fighting;
                    OnStartBattle();
                }
            }
            else if (m_state == EBattleState.Fighting)
            {
                m_battleField.Update(fDeltaTime);
            }

            if (GameCore.Instance.IsOfflineMode)
            {
                BattleServer.Instance.Update(fDeltaTime);
            }
        }

        public void HandleBattleCommand(BattleCommand cmd)
        {
            Debug.Log("HandleBattleCommand: " + cmd.ToString());
            m_battleField.AddCommad(cmd);
        }

        private void SendBattleCommand(BattleCommand cmd)
        {
            if (GameCore.Instance.IsOfflineMode)
            {
                BattleServer.Instance.AddRequest(cmd);
            }
            else
            {
                ProtoCSBattleCommand proto = new ProtoCSBattleCommand(cmd);
                NetworkMgr.Instance.Send(proto);
            }
        }

        public override void HandleMessage(ProtoBase proto)
        {
            switch (proto.MessageID)
            {
                case MessageId.SC_StartLevel:
                    {
                        ProtoSCStartLevel startLevel = proto as ProtoSCStartLevel;
                        GameCore.Instance.m_globalData.CurrLevel = startLevel.LevelId;
                        Reset();
                        SyncClock();
                        break;
                    }
                case MessageId.SC_LeaveLevel:
                    {
                        GameCore.Instance.SwitchToSubgame(ESubGame.SelectLevel);
                        break;
                    }
                case MessageId.SC_StartBattle:
                    {
                        ProtoSCStartBattle startBattle = proto as ProtoSCStartBattle;
                        m_state = EBattleState.Ready;
                        m_startFightTime = startBattle.StartTime + m_startFightDelay;
                        m_viewSceneTarget = startBattle.Translation;
                        Vector2 startPos = new Vector2(-6, m_viewSceneTarget.y);
                        m_battleField.BattleFieldObj.transform.position = startPos;
                        m_viewSceneSpeed = (m_viewSceneTarget.x + 6) / m_viewSceneTime;

                        for (int i = 0; i < startBattle.BallQueueA.Count; ++i)
                        {
                            m_battleField.m_ballReserve[0].Enqueue((ELevelBallType)startBattle.BallQueueA[i]);
                        }

                        m_battleField.CheckBallDisplay(0);

                        for (int i = 0; i < startBattle.BallQueueB.Count; ++i)
                        {
                            m_battleField.m_ballReserve[1].Enqueue((ELevelBallType)startBattle.BallQueueB[i]);
                        }

                        m_battleField.CheckBallDisplay(1);
                        if(GameCore.Instance.m_globalData.CurrLevel % 2 ==  1)
                        {
                            AudioMgr.PlayAudio(2, Guid.Empty);
                        }
                        else
                        {
                            AudioMgr.PlayAudio(3, Guid.Empty);
                        }
                        break;
                    }
                case MessageId.SC_EndBattle:
                    {
                        ProtoSCEndBattle endBattle = proto as ProtoSCEndBattle;
                        StartTimer(() =>
                        {
                            OnEndBattle(endBattle.IsWin);
                        }, 2f);
                        break;
                    }
                case MessageId.SC_BattleCommand:
                    {
                        ProtoSCBattleCommand battleCommand = proto as ProtoSCBattleCommand;
                        HandleBattleCommand(battleCommand.Command);
                        break;
                    }
            }
        }

        private bool IsChangeBallArea(Vector2 pos)
        {
            Vector2 ballPoint = m_battleField.BallPoint[GameCore.Instance.MySide];
            Vector2 exBallPoint = m_battleField.BallPoint[GameCore.Instance.MySide + 2];
            return Vector2.Distance(pos, new Vector2(ballPoint.x, exBallPoint.y)) <=
                   Mathf.Abs(ballPoint.x - exBallPoint.x);
        }

        private bool IsCanFireArea(Vector2 pos)
        {
            return Mathf.Abs(pos.y) < Mathf.Abs(m_battleField.BallPoint[GameCore.Instance.MySide].y);
        }

        public override void OnTouchStart(Gesture gesture)
        {
            base.OnTouchStart(gesture);
            if (m_battleField.IsExBall || m_battleField.IsBallUp || m_state != EBattleState.Fighting || m_battleField.IsWaitFireBall)
            {
                return;
            }
            Vector2 fingerPos = Camera.main.ScreenToWorldPoint(gesture.position);
            if ((!IsChangeBallArea(fingerPos) || !IsCanFireArea(fingerPos)))
            {
                if (m_battleField.MyBallNum > 0)
                {
                    m_isBeginFire = true;
                }
                else
                {
                    OnShowNoBallTip(fingerPos);
                    AudioMgr.PlayAudio(102);
                }
            }
        }

        public override void OnTouchDown(Gesture gesture)
        {
            base.OnTouchDown(gesture);
            if (m_isBeginFire)
            {
                Vector2 fingerPos = Camera.main.ScreenToWorldPoint(gesture.position);
                if (IsCanFireArea(fingerPos))
                {
                    if (gesture.actionTime > 0.1f)
                    {
                        m_battleLocalCal.CalFireBall(GameCore.Instance.MySide,
                            m_battleField.WaitBallsType[GameCore.Instance.MySide],
                            m_battleField.BallPoint[GameCore.Instance.MySide], fingerPos, 100, true);
                    }
                }
                else
                {
                    m_battleLocalCal.ClearLineRender();
                }
            }
        }

        public override void OnTouchUp(Gesture gesture)
        {
            base.OnTouchUp(gesture);
            if (m_isBeginFire)
            {
                Vector2 fingerPos = Camera.main.ScreenToWorldPoint(gesture.position);
                if (IsCanFireArea(fingerPos))
                {
                    m_battleField.IsWaitFireBall = true;
                    m_battleLocalCal.CalFireBall(GameCore.Instance.MySide, m_battleField.WaitBallsType[GameCore.Instance.MySide],
                        m_battleField.BallPoint[GameCore.Instance.MySide], fingerPos);
                }
                m_isBeginFire = false;
            }
        }

        public override void OnTap(Gesture gesture)
        {
            base.OnTap(gesture);
            if(m_battleField.IsExBall || m_battleField.IsBallUp || m_state != EBattleState.Fighting || m_battleField.IsWaitFireBall)
            {
                return;
            }
            if(IsChangeBallArea(Camera.main.ScreenToWorldPoint(gesture.position)) 
                && m_battleField.MyBallNum > 1)
            {
                if (GameCore.Instance.IsOfflineMode)
                {
                    HandleBattleCommand(new CmdExchangeBall(GameCore.Instance.MySide));
                }
                else
                {
                    ProtoCSBattleCommand pcmd = new ProtoCSBattleCommand(new CmdExchangeBall(GameCore.Instance.MySide));
                    NetworkMgr.Instance.Send(pcmd);
                }
            }
        }

        public void LeaveBattle()
        {
            ProtoCSLeaveLevel proto = new ProtoCSLeaveLevel();
            NetworkMgr.Instance.Send(proto);
        }

        public void NextLevel()
        {
            GameCore.Instance.m_globalData.CurrLevel = GameCore.Instance.m_globalData.CurrLevel + 1;
            RestartBattle();
        }

        public void RestartBattle()
        {
            ProtoCSStartLevel proto = new ProtoCSStartLevel(GameCore.Instance.m_globalData.CurrLevel);
            NetworkMgr.Instance.Send(proto);
        }

        private void Reset()
        {
            OnRestartBattle();
            m_battleField.Reset();
            m_state = EBattleState.None;
            m_startFightTime = 0;

            Vector2 startPos = new Vector2(-6, 0);
            m_battleField.BattleFieldObj.transform.position = startPos;

            LevelMapData data = LevelConfig.Instance.GetLevelMapData(GameCore.Instance.m_globalData.CurrLevel);
            m_battleField.InitLevel(data);
            m_battleLocalCal.InitLevel(data, m_battleField.Balls);

            m_battleField.IsWaitFireBall = false;
        }

        private void SyncClock()
        {
            m_state = EBattleState.SyncClock;
            ServerTimeHelper.Instance.SyncClock(OnSyncClockComplete);
        }

        private void OnSyncClockComplete()
        {
            m_state = EBattleState.Preparing;
            ProtoCSStartBattle proto = new ProtoCSStartBattle();
            NetworkMgr.Instance.Send(proto);
        }

        private void DoSetBallNum(int myBallNum, int mateBallBum)
        {
            OnSetBallNum(myBallNum, mateBallBum);
        }

        private void DoSetMissionProgress(int finishNum, int missionNum)
        {
            OnSetMissionProgress(finishNum, missionNum);
        }
    }
}
