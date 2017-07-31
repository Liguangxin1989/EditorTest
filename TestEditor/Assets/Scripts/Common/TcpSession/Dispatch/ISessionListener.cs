
//  ISessionListener.cs
//  Nilsen
//  2015-04-10

namespace NetWrapper.Network
{
    public interface ISessionListener
    {
        void OnConnect();
        void OnDisconnect();
        void OnPktReceive(StreamBuffer sb); //处理收包
        void OnDispatch();
    }
}