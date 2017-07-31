using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using NetWrapper.Network;
using NetWrapper;

namespace BubbleCouple
{
    public sealed class ProtoSCClockSync : ProtoBase
    {
        private ClockSync m_data;

        public int Step
        {
            get { return m_data.Step; }
        }

        public long ClientTime
        {
            get { return m_data.ClientTime; }
        }

        public long ServerTime
        {
            get { return m_data.ServerTime; }
        }

        public long Delta
        {
            get { return m_data.Delta; }
        }

        private ProtoSCClockSync() : base(MessageId.SC_ClockSync)
        {
            m_data = new ClockSync();
        }

        public ProtoSCClockSync(TCompactProtocol inProto) : this()
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
