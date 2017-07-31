using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetWrapper.Network;
using Thrift.Transport;
using Thrift.Protocol;

namespace BubbleCouple
{
    public static class ProtoFactory
    {
        delegate ProtoBase ThriftProtoConstructor(TCompactProtocol inProto);
        private static Dictionary<MessageId, ThriftProtoConstructor> s_protoConstructorDic;

        static ProtoFactory()
        {
            s_protoConstructorDic = new Dictionary<MessageId, ThriftProtoConstructor>
            {
                {MessageId.CS_EnterGame, (TCompactProtocol inProto) => { return new ProtoCSEnterGame(inProto); } },
                {MessageId.SC_EnterGame, (TCompactProtocol inProto) => { return new ProtoSCEnterGame(inProto); } },
                {MessageId.CS_StartLevel, (TCompactProtocol inProto) => { return new ProtoCSStartLevel(inProto); } },
                {MessageId.SC_StartLevel, (TCompactProtocol inProto) => { return new ProtoSCStartLevel(inProto); } },
                {MessageId.SC_EndBattle, (TCompactProtocol inProto) => { return new ProtoSCEndBattle(inProto); } },
                {MessageId.CS_BattleCommand, (TCompactProtocol inProto) => { return new ProtoCSBattleCommand(inProto); } },
                {MessageId.SC_BattleCommand, (TCompactProtocol inProto) => { return new ProtoSCBattleCommand(inProto); } },
                {MessageId.CS_ResetServer, (TCompactProtocol inProto) => { return new ProtoCSResetServer(inProto); } },
                {MessageId.SC_ResetServer, (TCompactProtocol inProto) => { return new ProtoSCResetServer(inProto); } },
                {MessageId.SC_MatchSuccess, (TCompactProtocol inProto) => { return new ProtoSCMatchSuccess(inProto); } },
                {MessageId.CS_ClockSync, (TCompactProtocol inProto) => { return new ProtoCSClockSync(inProto); } },
                {MessageId.SC_ClockSync, (TCompactProtocol inProto) => { return new ProtoSCClockSync(inProto); } },
                {MessageId.CS_StartBattle, (TCompactProtocol inProto) => { return new ProtoCSStartBattle(inProto); } },
                {MessageId.SC_StartBattle, (TCompactProtocol inProto) => { return new ProtoSCStartBattle(inProto); } },
                {MessageId.CS_LeaveLevel, (TCompactProtocol inProto) => { return new ProtoCSLeaveLevel(inProto); } },
                {MessageId.SC_LeaveLevel, (TCompactProtocol inProto) => { return new ProtoSCLeaveLevel(inProto); } },
                {MessageId.CS_LeaveGame, (TCompactProtocol inProto) => { return new ProtoCSLeaveGame(inProto); } },
                {MessageId.SC_LeaveGame, (TCompactProtocol inProto) => { return new ProtoSCLeaveGame(inProto); } },
                {MessageId.CS_JoinGame, (TCompactProtocol inProto) => { return new ProtoCSJoinGame(inProto); } },
                {MessageId.SC_JoinGame, (TCompactProtocol inProto) => { return new ProtoSCJoinGame(inProto); } },
            };
        }

        public static ProtoBase ConstructThriftProto(StreamBuffer sb)
        {
            MessageId messageId = (MessageId)sb.m_protocolHeader.ProtocolID;
            if (s_protoConstructorDic.ContainsKey(messageId))
            {
                TMemoryBuffer membuf = new TMemoryBuffer(sb.ByteBuffer);
                TCompactProtocol inProto = new TCompactProtocol(membuf);
                ProtoBase proto = s_protoConstructorDic[messageId].Invoke(inProto);
                return proto;
            }
            return null;
        }
    }
}
