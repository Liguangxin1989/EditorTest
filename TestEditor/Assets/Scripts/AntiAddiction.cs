using UnityEngine;
using System.Collections;
using System;
using Common;

namespace BubbleCouple
{
    public class AntiAddiction : Singleton<AntiAddiction>
    {
        private float m_antiAddictionTime = 0;
        private bool m_isCounting = false;

        private int m_antiAddictionCheckTime = 360;
        private int m_protectEyesCheckStep = 120;
        private int m_protectEyesCheckTime = 0;
        private bool m_isAntiAddictionOpen = false;
        public int AntiAddictionClearTime = 0;

        private bool IsAntiAddictionOpen
        {
            get
            {
                if (GameCore.Instance.IsOfflineMode)
                {
                    return false;
                }

                return m_isAntiAddictionOpen;
            }
        }

        public int AntiAddictionSeconds
        {
            get
            {
                if (m_isCounting)
                {
                    return (int)m_antiAddictionTime;
                }

                return GameCore.Instance.m_globalData.AntiAddictionSeconds;
            }
        }

        private AntiAddiction()
        {
            //m_isAntiAddictionOpen = false;
            //if (ConfigHelper.GetSysConfig(ESysConfig.AntiAddictionOpen) == 1)
            //{
            //    m_isAntiAddictionOpen = true;
            //}
            //m_protectEyesCheckStep = ConfigHelper.GetSysConfig(ESysConfig.ProtectEyesTime);
            //m_antiAddictionCheckTime = ConfigHelper.GetSysConfig(ESysConfig.AntiAddictionTime);
            //AntiAddictionClearTime = ConfigHelper.GetSysConfig(ESysConfig.AntiAddictionClear);
        }

        public void SetServerAntiAddictionFlag(bool isOpen)
        {
            m_isAntiAddictionOpen = isOpen;
        }

        public void Start()
        {
            if (!IsAntiAddictionOpen)
            {
                return;
            }

            if (m_isCounting == false)
            {
                m_antiAddictionTime = GameCore.Instance.m_globalData.AntiAddictionSeconds;
                m_isCounting = true;
                m_protectEyesCheckTime = (((int)m_antiAddictionTime / m_protectEyesCheckStep) + 1) * m_protectEyesCheckStep;
            }
        }

        public void Stop()
        {
            if (!IsAntiAddictionOpen)
            {
                return;
            }

            if (m_isCounting)
            {
                m_isCounting = false;
                GameCore.Instance.m_globalData.AntiAddictionSeconds = (int)m_antiAddictionTime;
            }
        }

        public void Update(float fDeltaTime)
        {
            if (!IsAntiAddictionOpen)
            {
                return;
            }

            if (m_isCounting)
            {
                m_antiAddictionTime += fDeltaTime;
            }
        }

        public bool IsNeedProtectEyes()
        {
            if (!IsAntiAddictionOpen)
            {
                return false;
            }

            if (AntiAddictionSeconds >= m_protectEyesCheckTime)
            {
                m_protectEyesCheckTime = ((AntiAddictionSeconds / m_protectEyesCheckStep) + 1) * m_protectEyesCheckStep;
                return true;
            }

            return false;
        }

        public bool IsNeedAntiAddiction()
        {
            if (!IsAntiAddictionOpen)
            {
                return false;
            }

            return AntiAddictionSeconds >= m_antiAddictionCheckTime;
        }
    }
}
