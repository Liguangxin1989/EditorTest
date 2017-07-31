using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoSCLeaveLevel : ProtoBase
    {
        private ScLeaveLevel m_data;

        private ProtoSCLeaveLevel() : base(MessageId.SC_LeaveLevel)
        {
            m_data = new ScLeaveLevel();
        }

        public ProtoSCLeaveLevel(TCompactProtocol inProto) : this()
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
