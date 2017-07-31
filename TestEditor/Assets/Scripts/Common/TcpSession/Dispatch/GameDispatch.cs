
//  GameDispatch.cs
//  Nilsen
//  2015-04-05

using NetWrapper.Network;

namespace NetWrapper
{
    /// <summary>
    /// 网络数据包调度
    /// </summary>
	class GameDispatch : DispatchBase
	{
        public GameDispatch(ISessionListener sessionListener, ISession session)
            : base(sessionListener, session)
        {
        }
	}
}
