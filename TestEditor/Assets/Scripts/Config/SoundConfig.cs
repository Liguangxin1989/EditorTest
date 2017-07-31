using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{ 
    public class SoundConfig
    {
        public class Sound
        {
            //文件名
            public string file;
            //中文描述
            public string des;
            //播放方式 0.播放1次 1.循环播放
            public int playMode;
            //类型 1.同类叠加 2.同类替换 3.同类忽略
            public int type;
            //音乐音效分组
            public int group;
        }

        private Dictionary<int, Sound> m_dic;

        public Dictionary<int, Sound> Dic
        {
            get { return m_dic; }
        }

        public SoundConfig()
        {
            m_dic = new Dictionary<int, Sound>();
        }

        public bool LoadSoundConfig(string[] strContent)
        {
            for (int index = 1; index < strContent.Length; ++index)
            {
                string[] strContents = strContent[index].Split(',');
                Sound tmp = new Sound();
                tmp.file = strContents[1];
                tmp.des = strContents[2];
                tmp.playMode = int.Parse(strContents[3]);
                tmp.type = int.Parse(strContents[4]);
                tmp.group = int.Parse(strContents[5]);
                m_dic.Add(int.Parse(strContents[0]), tmp);
            }

            return true;
        }
    }
}
