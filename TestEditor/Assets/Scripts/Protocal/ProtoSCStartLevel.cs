using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoSCStartLevel : ProtoBase
    {
        private ScStartLevel m_data;

        public int LevelId
        {
            get { return m_data.LevelId; }
        }

        private ProtoSCStartLevel() : base(MessageId.SC_StartLevel)
        {
            m_data = new ScStartLevel();
        }

        public ProtoSCStartLevel(TCompactProtocol inProto) : this()
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
