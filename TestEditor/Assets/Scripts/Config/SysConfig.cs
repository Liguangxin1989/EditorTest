using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{
    public enum ESysConfig
    {
        SceneLeftmostPosition = 1,
        BubbleFlyDuration = 2,
        BulletSwitchTime = 3,
        BubbleFallTime = 4,
        BubbleEraseTime = 5,
        SceneTranslateSpeed = 6,
        ViewSceneTime = 7
    }

    public class SysConfig
    {
        private Dictionary<ESysConfig, int> m_dic;

        public float SceneTransformSpeed
        {
            get { return GetSysConfig(ESysConfig.SceneTranslateSpeed) * 0.01f; }
        }

        public float BubbleFlyDuration
        {
            get { return GetSysConfig(ESysConfig.BubbleFlyDuration) * 0.001f; }
        }

        public bool LoadSysConfig(string[] strContent)
        {
            m_dic = new Dictionary<ESysConfig, int>();

            for (int index = 1; index < strContent.Length; ++index)
            {
                string[] strContents = strContent[index].Split(',');
                m_dic.Add((ESysConfig) (int.Parse(strContents[0])), int.Parse(strContents[2]));
            }

            return true;
        }

        public int GetSysConfig(ESysConfig item)
        {
            if (m_dic == null || !m_dic.ContainsKey(item))
            {
                return -1;
            }

            return m_dic[item];
        }
    }
}