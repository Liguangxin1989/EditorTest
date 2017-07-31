using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoCSJoinGame : ProtoBase
    {
        private CsJoinGame m_data;

        private ProtoCSJoinGame() : base(MessageId.CS_JoinGame)
        {
            m_data = new CsJoinGame();
        }

        public ProtoCSJoinGame(string roomId) : this()
        {
            m_data.Token = roomId;
        }

        public ProtoCSJoinGame(TCompactProtocol inProto) : this()
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
