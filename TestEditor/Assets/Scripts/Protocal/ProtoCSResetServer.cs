using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoCSResetServer : ProtoBase
    {
        private CsResetServer m_data;

        public ProtoCSResetServer() : base(MessageId.CS_ResetServer)
        {
            m_data = new CsResetServer();
        }

        public ProtoCSResetServer(TCompactProtocol inProto) : this()
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
