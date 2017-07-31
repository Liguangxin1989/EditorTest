using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class LineBubbleCollider : BubbleCollider
    {
        protected Vector2 m_start;
        protected Vector2 m_end;
        protected Vector2 m_normal;

        public LineBubbleCollider(Vector2 start, Vector2 end)
        {
            m_start = start;
            m_end = end;
            Vector2 e = end - start;
            m_normal = new Vector2(e.y, -e.x);
            m_normal.Normalize();
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

            CircleBubbleCollider cc1 = new CircleBubbleCollider(m_start, 0);
            if (cc1.RayCast(ray, out tempDistance))
            {
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    result = true;
                }
            }

            CircleBubbleCollider cc2 = new CircleBubbleCollider(m_end, 0);
            if (cc2.RayCast(ray, out tempDistance))
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
