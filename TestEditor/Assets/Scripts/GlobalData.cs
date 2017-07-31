using UnityEngine;
using System.Collections;
using Common;

namespace BubbleCouple
{
    public class GlobalData
    {
        private int m_MaxLevel;
        private int m_currLevel;

        private bool m_OpenMusic;
        private bool m_OpenSound;
        private bool m_isGuest;

        private string m_UniqueIdentifier;

        private int m_AntiAddictionTimeStamp;
        private int m_AntiAddictionSeconds;

        private int m_QuestionsCompletedTotal;
        private int m_QuestionsCompletedThisTime;

        private Vector2 m_ScreenLeftTopPoint;
        private Vector2 m_ScreenRightBottomPoint;

        public const float m_windowHight = 19.2f;
        public const float m_windowWidth = 10.8f;
        public const float m_ballRadius = 0.48f;
        public const float m_ballFallSpeed = 1f;
        public GlobalData()
        {
            SetMaxLevel(1);
            ReadData();
        }

        public int MaxLevel
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public string UniqueIdentifier
        {
            get
            {
                return SystemInfo.deviceUniqueIdentifier;
            }
        }

        public bool OpenMusic
        {
            get { return m_OpenMusic; }
            set
            {
                m_OpenMusic = value;
                SaveData();
            }
        }

        public bool OpenSound
        {
            get { return m_OpenSound; }
            set
            {
                m_OpenSound = value;
                SaveData();
            }
        }

        public int CurrLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; }
        }

        public int AntiAddictionSeconds
        {
            get
            {
                System.DateTime now = System.DateTime.Now;
                System.DateTime reftime = new System.DateTime(now.Year, now.Month, now.Day, AntiAddiction.Instance.AntiAddictionClearTime, 0, 0);
                int reftimestamp = TimeStamp.FromDateTime(reftime);
                if (now.Hour < AntiAddiction.Instance.AntiAddictionClearTime)
                {
                    int delta = reftimestamp - m_AntiAddictionTimeStamp;
                    if (delta <= 0 || delta > 24 * 3600)
                    {
                        m_AntiAddictionSeconds = 0;
                        m_AntiAddictionTimeStamp = TimeStamp.FromDateTime(now);
                        SaveData();
                    }
                }
                else
                {
                    int delta = m_AntiAddictionTimeStamp - reftimestamp;
                    if (delta < 0 || delta >= 24 * 3600)
                    {
                        m_AntiAddictionSeconds = 0;
                        m_AntiAddictionTimeStamp = TimeStamp.FromDateTime(now);
                        SaveData();
                    }
                }

                return m_AntiAddictionSeconds;
            }
            set
            {
                m_AntiAddictionSeconds = value;
                m_AntiAddictionTimeStamp = TimeStamp.FromDateTime(System.DateTime.Now);
                SaveData();
            }
        }

        public int QuestionsCompletedTotal
        {
            get { return m_QuestionsCompletedTotal; }
        }

        public int QuestionsCompletedThisTime
        {
            get { return m_QuestionsCompletedThisTime; }
        }

        public bool IsGuest
        {
            get { return m_isGuest; }
            set
            {
                m_isGuest = value;
                SaveData();
            }
        }

        public void SetMaxLevel(int maxLevel)
        {
            if (maxLevel > 0)
            {
                m_MaxLevel = maxLevel;
            }
            else
            {
                m_MaxLevel = 1;
            }
            m_currLevel = m_MaxLevel;
        }

        public void ClearData()
        {
            m_OpenMusic = true;
            m_OpenSound = true;
            m_AntiAddictionSeconds = 0;
            m_AntiAddictionTimeStamp = TimeStamp.FromDateTime(System.DateTime.Now);
            m_QuestionsCompletedTotal = 0;
            m_QuestionsCompletedThisTime = 0;
            SaveData();
        }

        private void ReadData()
        {
            m_OpenMusic = DoPlayerPrefs.GetBool("OpenMussic", true);
            m_OpenSound = DoPlayerPrefs.GetBool("OpenSound", true);
            m_isGuest = DoPlayerPrefs.GetBool("IsGuest", true);
            m_UniqueIdentifier = DoPlayerPrefs.GetString("UniqueIdentifier");
            if (m_UniqueIdentifier == "")
            {
                m_UniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
                DoPlayerPrefs.SetString("UniqueIdentifier", m_UniqueIdentifier);
            }
            m_AntiAddictionSeconds = DoPlayerPrefs.GetInt("AntiAddictionSeconds", 0);
            m_AntiAddictionTimeStamp = DoPlayerPrefs.GetInt("AntiAddictionTimeStamp", 0);
            m_QuestionsCompletedTotal = DoPlayerPrefs.GetInt("QuestionsCompletedTotal", 0);
            m_QuestionsCompletedThisTime = 0;
        }

        private void SaveData()
        {   
            DoPlayerPrefs.SetBool("OpenMussic", m_OpenMusic);
            DoPlayerPrefs.SetBool("OpenSound", m_OpenSound);
            DoPlayerPrefs.SetBool("IsGuest", m_isGuest);
            DoPlayerPrefs.SetString("UniqueIdentifier", m_UniqueIdentifier);
            DoPlayerPrefs.SetInt("AntiAddictionSeconds", m_AntiAddictionSeconds);
            DoPlayerPrefs.SetInt("AntiAddictionTimeStamp", m_AntiAddictionTimeStamp);
            DoPlayerPrefs.SetInt("QuestionsCompletedTotal", m_QuestionsCompletedTotal);
        }

        public void OnCompleteQuestion()
        {
            m_QuestionsCompletedTotal++;
            m_QuestionsCompletedThisTime++;
            SaveData();
        }
    }
}
