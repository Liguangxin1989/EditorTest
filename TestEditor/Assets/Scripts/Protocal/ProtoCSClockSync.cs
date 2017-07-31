using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using NetWrapper;
using NetWrapper.Network;

namespace BubbleCouple
{
    public sealed class ProtoCSClockSync : ProtoBase
    {
        private ClockSync m_data;

        private ProtoCSClockSync() : base(MessageId.CS_ClockSync)
        {
            m_data = new ClockSync();
        }

        public ProtoCSClockSync(int step, long clientTime = 0, long serverTime = 0) : this()
        {
            m_data.Step = step;
            m_data.ClientTime = clientTime;
            m_data.ServerTime = serverTime;
            m_data.Delta = 0;
        }

        public ProtoCSClockSync(TCompactProtocol inProto) : this()
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
