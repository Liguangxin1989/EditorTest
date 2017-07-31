using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class VerticleBubbleCollider : LineBubbleCollider
    {
        public VerticleBubbleCollider(float x, float y1, float y2) : base(new Vector2(x, y1), new Vector2(x, y2))
        {

        }

        public override bool RayCast(BubbleRay ray, out float distance)
        {
            distance = float.MaxValue;
            bool result = false;
            Vector2 vp = ray.Orign - m_start;
            if (Vector2.Dot(vp, m_normal) <= 0)
            {
                return false;
            }

            Vector2 v1 = m_start + m_normal * ray.Radius;
            Vector2 v2 = m_end + m_normal * ray.Radius;

            float tempDistance;
            if (ray.IntersectionSegment(v1, v2, out tempDistance))
            {
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    result = true;
                }
            }

            return result;
        }
    }
}
