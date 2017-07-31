using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoSCJoinGame : ProtoBase
    {
        private ScJoinGame m_data;

        public JoinGameResult Result
        {
            get { return (JoinGameResult)m_data.Result; }
        }
        public int Count
        {
            get
            {
                return m_data.Count;
            }
        }

        private ProtoSCJoinGame() : base(MessageId.SC_JoinGame)
        {
            m_data = new ScJoinGame();
        }

        public ProtoSCJoinGame(TCompactProtocol inProto) : this()
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
