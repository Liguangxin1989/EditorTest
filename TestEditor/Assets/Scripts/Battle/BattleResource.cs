using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace BubbleCouple
{
    public class BattleResource : Singleton<BattleResource>
    {
        private Dictionary<ELevelBallType, GameObject> m_ballResources = new Dictionary<ELevelBallType, GameObject>();
        private bool bLoaded = false;

        private BattleResource()
        {
        }

        public void LoadResources()
        {
            if (bLoaded)
            {
                return;
            }

            foreach(var type in System.Enum.GetValues(typeof(ELevelBallType)))
            {
                string name = System.Enum.GetName(typeof(ELevelBallType), type);
                GameObject res = Resources.Load<GameObject>("Prefabs/" + name);
                if (res != null)
                {
                    m_ballResources.Add((ELevelBallType)type, res);
                }
            }

            bLoaded = true;
        }

        public GameObject GetBallResource(ELevelBallType type)
        {
            if (m_ballResources.ContainsKey(type))
            {
                return m_ballResources[type];
            }

            return null;
        }
    }
}
