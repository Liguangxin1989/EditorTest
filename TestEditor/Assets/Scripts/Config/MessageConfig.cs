using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{
    public class MessageConfig
    {
        public class Message
        {
            //位置 0 UI内 1 提示位
            public int locate;
            //提示图片
            public string image;
            //停留时长 0 永久
            public float time;
            //文字
            public string des;
            //音效
            public int sound;
        }

        private Dictionary<int, Message> m_dic;

        public Dictionary<int, Message> Dic
        {
            get { return m_dic; }
        }

        public MessageConfig()
        {
            m_dic = new Dictionary<int, Message>();
        }

        public bool LoadMessageConfig(string[] strContent)
        {
            for (int index = 1; index < strContent.Length; ++index)
            {
                string[] strContents = strContent[index].Split(',');
                Message tmp = new Message();
                tmp.locate = int.Parse(strContents[1]);
                tmp.image = strContents[2];
                tmp.time = int.Parse(strContents[3])*0.001f;
                tmp.des = strContents[4];
                tmp.sound = int.Parse(strContents[5]);
                m_dic.Add(int.Parse(strContents[0]), tmp);
            }

            return true;
        }
    }
}
