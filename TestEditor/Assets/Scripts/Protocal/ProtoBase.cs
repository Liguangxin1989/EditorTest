using UnityEngine;
using System.Collections;
using NetWrapper.Network;
using Thrift.Protocol;
using Thrift.Transport;
using System.Collections.Generic;

namespace BubbleCouple
{
    public abstract class ProtoBase
    {
        #region regist protocol
        //private static Dictionary<MessageId, System.Reflection.ConstructorInfo> s_protocalDic;

        //static ProtoBase()
        //{
        //    s_protocalDic = new Dictionary<MessageId, System.Reflection.ConstructorInfo>();
        //    RegistAllProtocals();
        //}

        //private static void RegistAllProtocals()
        //{
        //    System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
        //    foreach (var type in types)
        //    {
        //        if (type.BaseType == typeof(ProtoBase))
        //        {
        //            System.Reflection.ConstructorInfo[] constructors = type.GetConstructors(System.Reflection.BindingFlags.Instance |
        //                                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        //            foreach (System.Reflection.ConstructorInfo constructor in constructors)
        //            {
        //                System.Reflection.ParameterInfo[] parameterInfoArray = constructor.GetParameters();
        //                if (0 == parameterInfoArray.Length)
        //                {
        //                    ProtoBase proto = constructor.Invoke(null) as ProtoBase;
        //                    s_protocalDic[proto.MessageID] = constructor;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static ProtoBase CreateInstance(StreamBuffer sb)
        //{
        //    MessageId messageId = (MessageId)sb.m_protocolHeader.ProtocolID;
        //    TMemoryBuffer membuf = new TMemoryBuffer(sb.ByteBuffer);
        //    TCompactProtocol inProto = new TCompactProtocol(membuf);
        //    if (s_protocalDic.ContainsKey(messageId))
        //    {
        //        ProtoBase proto = s_protocalDic[messageId].Invoke(null) as ProtoBase;
        //        proto.Read(inProto);
        //        return proto;
        //    }

        //    return null;
        //}

        #endregion

        private MessageId _messageId;
        public MessageId MessageID
        {
            get { return _messageId; }
        }

        public ProtoBase(MessageId msgId)
        {
            _messageId = msgId;
        }

        protected virtual void Read(TCompactProtocol inProto)
        {

        }

        protected virtual void Write(TCompactProtocol outProto)
        {
        }

        public StreamBuffer GetStream()
        {
            TMemoryBuffer membuf = new TMemoryBuffer();
            TCompactProtocol protocal = new TCompactProtocol(membuf);
            Write(protocal);
            byte[] buf = membuf.GetBuffer();
            StreamBuffer stream = new StreamBuffer((int)_messageId, buf);
            return stream;
        }

        public virtual bool IsRoomMessage()
        {
            return false;
        }

        public override string ToString()
        {
            return "MessageId: " + _messageId.ToString();
        }
    }
}

