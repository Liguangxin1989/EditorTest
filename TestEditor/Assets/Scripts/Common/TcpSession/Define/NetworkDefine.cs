//  NetworkDefine.cs
//  Nilsen
//  2015-04-08

#define GeneralHeader
using System;

namespace NetWrapper.Network
{
    // 网络状态
    public enum SESSION_STATUS
    { 
        NO_CONNECT = 0,     //无连接
        CONNECT_SUCCESS = 1,    //连接成功
        CONNECT_FAILED_CONNECT_ERROR = 2,   //连接失败
        CONNECT_FAILED_TIME_OUT = 3,        //连接超时
        CONNECT_EXIT = 4,                   //连接退出
        RE_CONNECT = 5,     //重新连接
        EXCEPTION_ON_RECEIVE, //接收数据出现异常
        EXCEPTION_ON_SEND, // 发送数据出现异常
    }

    public enum SocketState
    {
        Disconnected = 0,
        Connecting,
        Connected,
        Disconnecting,
        Noconnect,
    }

    // 包执行结果
    public enum PACKET_EXC_RES
    { 
        PACKET_EXC_ERROR = 0,       //执行错误
        PACKET_EXC_BREAK,           //执行终止
        PACKET_EXC_CONTINUE,        //执行继续
        PACKET_EXC_NOTREMOVE,       //不删除
        PACKET_EXC_NOTREMOVE_ERROR, //不删除错误
        PACKET_EXC_CANNOT_FIND_HANDLE,  //找不到对应执行句柄
    }

    // 请求操作结果
    public enum REQUIRE_RESULT
    {
        REQUIRE_SUCCESS,   //成功 
        REQUIRE_OP_FAILS,   //操作失败
        REQUIRE_SERVER_BUSY,    //服务器忙，重试
        REQUIRE_OP_TIMES,   //操作过于频繁
    }

    // 网络定义端点
    public class ENDPOINT
    {
        public const string Server_IP = "192.168.1.50";
        public const int Server_Port = 6001;
    }

    public enum DELIVERY_TYPE
    {
        UNRELIABLE = 0,
        RELIABLE_ORDERED = 1,
        DELIVERY_NORMAL = 2,
    }

    public class ProtocolHeader
    {
        private int m_errCode;
        private int m_otherData;
        private readonly int m_protocolID;
        private int m_protocolLength;
        public static int HeadLength
        {
#if GeneralHeader
            get { return 12; }
#else
            get { return 4; }
#endif
        }

        public static int MaxHeadLength
        {
            get { return 20480; }
        }

        public int ErrorCode
        {
            get { return m_errCode; }
            set { m_errCode = value; }
        }

        public int ProtocolID
        {
            get { return m_protocolID; }
            //set { m_protocolID = value; }
        }

        internal int BodyLength
        {
            get { return m_protocolLength; }
            set { m_protocolLength = value; }
        }

        public int ExtraData
        {
            get { return m_otherData; }
            set { m_otherData = value; }
        }

        public ProtocolHeader()
        {
            m_errCode = 0;
            m_otherData = 0;
            m_protocolID = -10;
            m_protocolLength = 0;
        }

        public ProtocolHeader(int protocolID)
        {
            m_errCode = 0;
            m_otherData = 0;
            m_protocolID = protocolID;
            m_protocolLength = 0;
        }

        public ProtocolHeader(byte[] headerStream, int offset = 0)
        {
            if (headerStream.Length - offset < HeadLength)
            {
                throw new ArgumentException(string.Format("the stream length must be equal to HeadLength, stream length {0}, HeadLength {}",
                    headerStream.Length, HeadLength), "headerStream");
            }
#if GeneralHeader
            int errCode = BitConverter.ToInt32(headerStream, offset);
            int protocolID = BitConverter.ToInt32(headerStream, offset + 4);
            int protocolLength = BitConverter.ToInt32(headerStream, offset + 8);
            m_errCode = Tool.NetUtil.NetworkToHostOrder(errCode);
            m_protocolID = Tool.NetUtil.NetworkToHostOrder(protocolID);
            m_protocolLength = Tool.NetUtil.NetworkToHostOrder(protocolLength);
#else
            int protocolLength = BitConverter.ToInt32(headerStream, offset);
            m_protocolLength = Tool.NetUtil.NetworkToHostOrder(protocolLength);
#endif
        }

        public byte[] GetHeaderStream()
        {
            byte[] headerStream = new byte[HeadLength];
            int errCode = Tool.NetUtil.HostToNetworkOrder(m_errCode);
            int protocolID = Tool.NetUtil.HostToNetworkOrder(m_protocolID);
            int protocolLength = Tool.NetUtil.HostToNetworkOrder(m_protocolLength);
#if GeneralHeader
            byte[] retBytes = BitConverter.GetBytes(errCode);          
            byte[] protoBytes = BitConverter.GetBytes(protocolID);
            byte[] lengthBytes = BitConverter.GetBytes(protocolLength);
            Array.Copy(retBytes, 0, headerStream, 0, retBytes.Length);    
            Array.Copy(protoBytes, 0, headerStream, 4, protoBytes.Length);
            Array.Copy(lengthBytes, 0, headerStream, 8, lengthBytes.Length);
#else
            byte[] lengthBytes = BitConverter.GetBytes(protocolLength);
            Array.Copy(lengthBytes, 0, headerStream, 0, lengthBytes.Length);
#endif
            return headerStream;
        }
    }
}