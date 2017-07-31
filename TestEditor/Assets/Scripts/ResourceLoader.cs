
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BubbleCouple
{
    public delegate void OnResLoad<T>(string fileName, T resObj) where T : UnityEngine.Object;

    public class ResourceLoader : MonoBehaviour
    {
        private Dictionary<string, UnityEngine.Object> m_pathToResourceMap = new Dictionary<string, Object>();
            // 路径到资源映射(包含文件名) 

        private Dictionary<string, int> m_resourceRefCout = new Dictionary<string, int>();

        public void LoadResourceAsync<T>(string resName, OnResLoad<T> resLoader) where T : UnityEngine.Object
        {
            if (m_pathToResourceMap.ContainsKey(resName))
            {
                m_resourceRefCout[resName] += 1;

                resLoader(resName, (T) m_pathToResourceMap[resName]);
            }
            else
            {
                StartCoroutine(DoLoadResourceAsync(resName, resLoader));
            }
        }

        private IEnumerator DoLoadResourceAsync<T>(string resName, OnResLoad<T> resLoader) where T : UnityEngine.Object
        {
            ResourceRequest req = Resources.LoadAsync(resName);
            yield return req;

            if (!m_pathToResourceMap.ContainsKey(resName))
            {
                m_pathToResourceMap.Add(resName, req.asset);
                m_resourceRefCout[resName] = 1;
            }
            else
            {
                m_resourceRefCout[resName] += 1;
            }

            resLoader(resName, (T) req.asset);
        }

        public UnityEngine.Object LoadResource<T>(string resName) where T : UnityEngine.Object
        {
            UnityEngine.Object resource = null;
            if (m_pathToResourceMap.ContainsKey(resName))
            {
                m_resourceRefCout[resName] += 1;
                resource = m_pathToResourceMap[resName] as T;
            }
            else
            {
                resource = Resources.Load(resName, typeof (T));
                Debug.Log(resName + " " + resource);
                m_resourceRefCout[resName] = 1;
                m_pathToResourceMap[resName] = resource;
            }

            return resource;
        }

        public void UnLoadResource(string resName)
        {
            if (m_resourceRefCout.ContainsKey(resName))
            {
                if (m_resourceRefCout[resName] > 0)
                {
                    m_resourceRefCout[resName] -= 1;
                }

                if (m_resourceRefCout[resName] == 0)
                {
                    Resources.UnloadAsset(m_pathToResourceMap[resName]);
                    m_pathToResourceMap.Remove(resName);
                    m_resourceRefCout.Remove(resName);
                }
            }
        }
    }
}
