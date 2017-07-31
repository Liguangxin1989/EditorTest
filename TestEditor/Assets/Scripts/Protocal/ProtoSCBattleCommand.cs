using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using System.Collections.Generic;

namespace BubbleCouple
{
    public sealed class ProtoSCBattleCommand : ProtoBase
    {
        private StructBattleCommand m_data;
        private BattleCommand m_command;

        public BattleCommand Command
        {
            get { return m_command; }
        }

        private ProtoSCBattleCommand() : base(MessageId.SC_BattleCommand)
        {
            m_data = new StructBattleCommand();
        }

        public ProtoSCBattleCommand(TCompactProtocol inProto) : this()
        {
            Read(inProto);
        }

        protected override void Read(TCompactProtocol inProto)
        {
            base.Read(inProto);
            m_data.Read(inProto);
            m_command = BattleCommand.CreateInstance(m_data);
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
