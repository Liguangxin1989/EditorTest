using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class CircleBubbleCollider : BubbleCollider
    {
        private Vector2 m_center;
        private float m_radius;

        public CircleBubbleCollider(Vector2 center, float radius)
        {
            m_center = center;
            m_radius = radius;
        }

        public override bool RayCast(BubbleRay ray, out float distance)
        {
            distance = float.MaxValue;
            bool result = false;
            Vector2 orignToCenter = m_center - ray.Orign;
            float proj = Vector2.Dot(orignToCenter, ray.Direction);
            if (proj >= 0)
            {
                Vector2 p = ray.Ray.GetPoint(proj);
                Vector2 cp = m_center - p;
                float cpdis = cp.magnitude;
                float sumRadius = m_radius + ray.Radius;
                if (cpdis == sumRadius)
                {
                    distance = proj;
                    result = true;
                }
                else if (cpdis < sumRadius)
                {
                    float delta = Mathf.Sqrt(sumRadius * sumRadius - cp.sqrMagnitude);
                    distance = proj - delta;
                    result = true;
                }
            }

            return result;
        }
    }
}
