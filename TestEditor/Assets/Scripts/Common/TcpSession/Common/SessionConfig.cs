using System.Net.Sockets;

namespace NetWrapper.Network
{
    public class SessionConfig
    {
        public bool nodeley = true;
        public int dendBufferSize = 8192;
        public int dendTimeout = 0;
        public int recBufferSize = 8192;
        public int receiveTimeout = 0;
        public int disconnectTimeout = 10000;
        public AddressFamily addressFamily;
    }
}