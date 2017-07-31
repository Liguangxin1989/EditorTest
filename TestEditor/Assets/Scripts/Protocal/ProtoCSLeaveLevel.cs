using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoCSLeaveLevel : ProtoBase
    {
        private CsLeaveLevel m_data;

        public ProtoCSLeaveLevel() : base(MessageId.CS_LeaveLevel)
        {
            m_data = new CsLeaveLevel();
        }

        public ProtoCSLeaveLevel(TCompactProtocol inProto) : this()
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
