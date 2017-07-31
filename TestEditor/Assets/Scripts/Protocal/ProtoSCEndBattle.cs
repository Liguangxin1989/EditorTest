using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoSCEndBattle : ProtoBase
    {
        private ScEndBattle m_data;

        public bool IsWin
        {
            get { return m_data.IsWin; }
        }

        private ProtoSCEndBattle() : base(MessageId.SC_EndBattle)
        {
            m_data = new ScEndBattle();
        }

        public ProtoSCEndBattle(TCompactProtocol inProto) : this()
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
