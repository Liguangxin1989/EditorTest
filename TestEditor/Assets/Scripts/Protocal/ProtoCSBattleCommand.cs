using UnityEngine;
using System.Collections;
using Thrift.Protocol;

namespace BubbleCouple
{
    public sealed class ProtoCSBattleCommand : ProtoBase
    {
        private StructBattleCommand m_data;

        private ProtoCSBattleCommand() : base(MessageId.CS_BattleCommand)
        {
            m_data = new StructBattleCommand();
        }

        public ProtoCSBattleCommand(BattleCommand cmd) : this()
        {
            m_data = cmd.CmdData;
        }

        public ProtoCSBattleCommand(TCompactProtocol inProto) : this()
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
            return base.ToString();
        }
    }
}
