using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{
    public class ConfigHelper : Singleton<ConfigHelper>
    {
        private ConfigHelper()
        {
            m_configMgr = GameConfigMgr.Instance;
        }

        private readonly GameConfigMgr m_configMgr;

        public static int GetSysConfig(ESysConfig type)
        {
            return Instance.DoGetSysConfig(type);
        }

        public static SoundConfig.Sound GetSoundInfor(int id)
        {
            return Instance.DoGetSoundInfor(id);
        }

        public static MessageConfig.Message GetMessageInfor(int id)
        {
            return Instance.DoGetMessageInfor(id);
        }

        public static float GetSceneTransformSpeed()
        {
            return Instance.DoGetSceneTransformSpeed();
        }

        public static float GetBubbleFlyDuration()
        {
            return Instance.DoGetBubbleFlyDuration();
        }

        public static LevelConfig.LevelInfo GetLevelInfo(int levelId)
        {
            return LevelConfig.Instance.GetLevelInfo(levelId);
        }

        //-------
        private int DoGetSysConfig(ESysConfig type)
        {
            return m_configMgr.m_SysConfig.GetSysConfig(type);
        }

        private SoundConfig.Sound DoGetSoundInfor(int id)
        {
            if (!m_configMgr.m_SoundConfig.Dic.ContainsKey(id))
            {
                return null;
            }

            return m_configMgr.m_SoundConfig.Dic[id];
        }

        private MessageConfig.Message DoGetMessageInfor(int id)
        {
            if (!m_configMgr.m_MessageConfig.Dic.ContainsKey(id))
            {
                return null;
            }
            return m_configMgr.m_MessageConfig.Dic[id];
        }

        private float DoGetSceneTransformSpeed()
        {
            return m_configMgr.m_SysConfig.SceneTransformSpeed;
        }

        private float DoGetBubbleFlyDuration()
        {
            return m_configMgr.m_SysConfig.BubbleFlyDuration;
        }
    }
}
