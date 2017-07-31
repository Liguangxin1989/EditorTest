using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoSCResetServer : ProtoBase
    {
        private ScResetServer m_data;

        private ProtoSCResetServer() : base(MessageId.SC_ResetServer)
        {
            m_data = new ScResetServer();
        }

        public ProtoSCResetServer(TCompactProtocol inProto) : this()
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
