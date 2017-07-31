using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BubbleCouple
{
    public interface IMessageHandler
    {

        void OnConnect();
        void OnDisconnect();
        void HandleMessage(ProtoBase proto);
    }
}
