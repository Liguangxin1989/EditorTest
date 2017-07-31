using UnityEngine;
using System.Collections;

namespace BubbleCouple
{
    public abstract class UIBridge
    {
        protected UIBase m_ui = null;
        protected SubGame m_subGame = null;

        public UIBridge(SubGame subGame, UIBase ui)
        {
            m_subGame = subGame;
            m_ui = ui;
        }

        virtual public void Init()
        {
            if (m_ui != null)
                m_ui.Init(this);
        }

        virtual public void Update(float deltaTime)
        {
            if (m_ui != null)
                m_ui.Update(deltaTime);
        }

        virtual public void OnAdaptResolution()
        {
            if (m_ui != null)
                m_ui.OnAdaptResolution();
        }

        virtual public void Destroy()
        {
            if (m_ui != null)
                m_ui.Destroy();
            m_ui = null;
            m_subGame = null;
        }
    }
}
