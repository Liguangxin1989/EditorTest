using UnityEngine;
using System.Collections;

namespace BubbleCouple
{
    public abstract class UIBase
    {
        protected UIBridge m_bridge = null;
        public UIBase()
        {
        }

        virtual public void Init(UIBridge bridge)
        {
            m_bridge = bridge;
        }

        virtual public void Update(float deltaTime)
        {
        }

        virtual public void OnAdaptResolution()
        {

        }

        virtual public void Destroy()
        {

        }
    }
}
