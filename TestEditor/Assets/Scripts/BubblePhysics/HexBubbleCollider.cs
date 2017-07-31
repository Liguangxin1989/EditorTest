using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class HexBubbleCollider : BubbleCollider
    {
        private List<Vector2> m_verticles;

        public HexBubbleCollider(List<Vector2> verticles)
        {
            m_verticles = verticles;
        }

        public override bool RayCast(BubbleRay ray, out float distance)
        {
            distance = float.MaxValue;
            bool result = false;

            if (m_verticles.Count != 6)
            {
                return false;
            }

            for (int i = 0; i < m_verticles.Count; i++)
            {
                Vector2 start, end;
                start = m_verticles[i];
                if (i == m_verticles.Count - 1)
                {
                    end = m_verticles[0];
                }
                else
                {
                    end = m_verticles[i + 1];
                }

                float tempDistance;
                LineBubbleCollider lc = new LineBubbleCollider(start, end);
                if (lc.RayCast(ray, out tempDistance))
                {
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
