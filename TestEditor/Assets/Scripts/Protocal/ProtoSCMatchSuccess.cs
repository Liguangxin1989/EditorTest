using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoSCMatchSuccess : ProtoBase
    {
        private ScMatchSuccess m_data;

        private ProtoSCMatchSuccess() : base(MessageId.SC_MatchSuccess)
        {
            m_data = new ScMatchSuccess();
        }

        public ProtoSCMatchSuccess(TCompactProtocol inProto) : this()
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
