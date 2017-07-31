using UnityEngine;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{
    public sealed class GameResourceMgr
    {
        private ResourceLoader m_loader;

        public GameResourceMgr()
        {
            m_loader = ObjectEX.CreatGOWithBehaviour<ResourceLoader>("GameResourceMgrObj");
        }

        public T GetResourceByName<T>(string resName) where T : UnityEngine.Object
        {
            T resourceGo = null;

            UnityEngine.Object obj = m_loader.LoadResource<T>(resName);
            if (obj != null)
            {
                resourceGo = (T) obj;
            }
            else
            {
                Debug.LogWarning(resName + " not founded");
            }

            return resourceGo;
        }

        public void GetResourceAsync<T>(string resName, OnResLoad<T> resLoader) where T : UnityEngine.Object
        {
            m_loader.LoadResourceAsync<T>(resName, resLoader);
        }

        public void ReleaseResourceByName(string resName)
        {
            m_loader.UnLoadResource(resName);
        }
    }
}