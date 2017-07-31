using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoSCEnterGame : ProtoBase
    {
        private ScEnterGame m_data;

        public bool IsSuccess
        {
            get { return m_data.Success; }
        }
        public int Side
        {
            get
            {
                if (m_data.Success)
                {
                    return m_data.Side;
                }
                return 0;
            }
        }

        private ProtoSCEnterGame() : base(MessageId.SC_EnterGame)
        {
            m_data = new ScEnterGame();
        }

        public ProtoSCEnterGame(TCompactProtocol inProto) : this()
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
