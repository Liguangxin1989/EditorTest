using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using System.Collections.Generic;

namespace BubbleCouple
{
    public sealed class ProtoSCStartBattle : ProtoBase
    {
        private ScStartBattle m_data;

        public long StartTime
        {
            get { return m_data.StartTime; }
        }
        public Vector2 Translation
        {
            get
            {
                return new Vector2((float)m_data.SceneTransform.Translation.X, (float)m_data.SceneTransform.Translation.Y);
            }
        }

        public float Rotation
        {
            get
            {
                return (float)m_data.SceneTransform.Rotation;
            }
        }

        public int BallCountA
        {
            get
            {
                return m_data.BallQueueA.Count;
            }
        }

        public List<int> BallQueueA
        {
            get
            {
                return m_data.BallQueueA;
            }
        }

        public int BallCountB
        {
            get
            {
                return m_data.BallQueueB.Count;
            }
        }

        public List<int> BallQueueB
        {
            get
            {
                return m_data.BallQueueB;
            }
        }

        private ProtoSCStartBattle() : base(MessageId.SC_StartBattle)
        {
            m_data = new ScStartBattle();
        }

        public ProtoSCStartBattle(TCompactProtocol inProto) : this()
        {
            Read(inProto);
        }

        protected override void Read(TCompactProtocol inProto)
        {
            base.Read(inProto);
            m_data.Read(inProto);
        }

        protected override void Write(TCompactProtocol outProto)
        {
            base.Write(outProto);
            m_data.Write(outProto);
        }

        public override string ToString()
        {
            return base.ToString() + ", data: " + m_data.ToString();
        }
    }
}
