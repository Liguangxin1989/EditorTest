using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoCSLeaveGame : ProtoBase
    {
        private CsLeaveGame m_data;

        public ProtoCSLeaveGame() : base(MessageId.CS_LeaveGame)
        {
            m_data = new CsLeaveGame();
        }

        public ProtoCSLeaveGame(TCompactProtocol inProto) : this()
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
