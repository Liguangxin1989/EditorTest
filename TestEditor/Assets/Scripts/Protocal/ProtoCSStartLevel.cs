using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoCSStartLevel : ProtoBase
    {
        private CsStartLevel m_data;

        private ProtoCSStartLevel() : base(MessageId.CS_StartLevel)
        {
            m_data = new CsStartLevel();
        }

        public ProtoCSStartLevel(int levelId) : this()
        {
            m_data.LevelId = levelId;
        }

        public ProtoCSStartLevel(TCompactProtocol inProto) : this()
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
