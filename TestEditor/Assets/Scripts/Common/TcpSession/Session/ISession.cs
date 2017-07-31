//  ISession.cs
//  Nilsen import
//  2015-4-8

namespace NetWrapper.Network
{
    /// <summary>
    /// 会话接口
    /// </summary>
    public interface ISession
    {
        void Connect(string address, int port);

        void DisConnect();

        void Send(RawStreamBuffer sb);

        void Update();
    }
}