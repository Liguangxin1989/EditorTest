using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using Common;

namespace BubbleCouple
{
    public enum ESubGame : byte
    {
        SplashScreen = 0,
        Login,
        Match,
        Battle,
        SelectLevel,

        Num,
        None = byte.MaxValue
    }

    public abstract class SubGame
    {
        public readonly ESubGame m_subGameType;
        protected bool m_bSceneLoaded = false;
        private bool m_bPreAdaptResolution;
        private bool m_bAdaptResolution;
        private UIBridge m_bridge = null;
        private Timer m_timer;

        public SubGame(ESubGame type)
        {
            m_subGameType = type;
            m_bSceneLoaded = false;
            m_bPreAdaptResolution = false;
            m_bAdaptResolution = false;
            m_timer = new Timer();
        }

        public virtual void Init()
        {

        }

        public virtual void Update(float fDeltaTime)
        {
            if (m_bSceneLoaded == false)
                return;

            if(m_timer != null)
            {
                m_timer.Update(fDeltaTime);
            }

            if (m_bridge != null)
            {
                m_bridge.Update(fDeltaTime);
            }

            if(m_bPreAdaptResolution)
            {
                m_bPreAdaptResolution = false;
                m_bAdaptResolution = true;
            }
            else
            {
                if(m_bAdaptResolution)
                {
                    m_bAdaptResolution = false;
                    OnAdaptResolution();
                }
            }
        }

        public virtual void Destroy()
        {
            if (m_bridge != null)
            {
                m_bridge.Destroy();
                m_bridge = null;
            }
            AudioMgr.ClearAudioComponent();
        }

        public virtual void OnPause(bool status)
        {

        }

        public virtual void OnSceneLoaded(bool bLoaded)
        {
            m_bridge = CreateUIBridge();
            if (m_bridge != null)
            {
                m_bridge.Init();
            }
            m_bSceneLoaded = true;
            m_bPreAdaptResolution = true;
            m_bAdaptResolution = false;
        }

        protected virtual UIBridge CreateUIBridge()
        {
            return null;
        }

        public virtual void OnTap( Gesture gesture)
        {

        }

        public virtual void OnDrag(Gesture gesture)
        {

        }

        public virtual void OnDragStart(Gesture gesture)
        {

        }

        public virtual void OnDragEnd(Gesture gesture)
        {

        }

        public virtual void OnTouchStart(Gesture gesture)
        {
            
        }

        public virtual void OnTouchDown(Gesture gesture)
        {

        }

        public virtual void OnTouchUp(Gesture gesture)
        {

        }

        protected virtual void OnAdaptResolution()
        {
            if (m_bridge != null)
            {
                m_bridge.OnAdaptResolution();
            }
        }

        public virtual void OnDisconnect()
        {

        }

        public virtual void HandleMessage(ProtoBase proto)
        {

        }

        public uint StartTimer(Action func, float fTime)
        {
            return m_timer.StartTimer(func, fTime);
        }

        public void StopTimer(uint timerID)
        {
            m_timer.StopTimer(timerID);
        }
    }
}
