using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class BubbleRay
    {
        private Ray2D m_ray;
        private float m_radius;

        public Vector2 Orign
        {
            get { return m_ray.origin; }
        }

        public Vector2 Direction
        {
            get { return m_ray.direction; }
        }

        public Ray2D Ray
        {
            get { return m_ray; }
        }

        public float Radius
        {
            get { return m_radius; }
        }

        public BubbleRay(Vector2 orign, Vector2 direction, float radius)
        {
            m_ray = new Ray2D(orign, direction.normalized);
            m_radius = radius;
        }

        public bool IntersectionSegment(Vector2 start, Vector2 end, out float distance)
        {
            distance = float.MaxValue;

            // Put the ray into the edge's frame of reference.
            Vector2 p1 = Ray.origin;
            Vector2 d = Ray.direction;

            Vector2 v1 = start;
            Vector2 v2 = end;
            Vector2 e = v2 - v1;
            Vector2 normal = new Vector2(e.y, -e.x);
            normal.Normalize();

            // q = p1 + t * d
            // dot(normal, q - v1) = 0
            // dot(normal, p1 - v1) + t * dot(normal, d) = 0
            float numerator = Vector2.Dot(normal, v1 - p1);
            float denominator = Vector2.Dot(normal, d);

            if (denominator == 0.0f)
            {
                return false;
            }

            float t = numerator / denominator;
            if (t < 0.0f)
            {
                return false;
            }

            Vector2 q = p1 + t * d;

            // q = v1 + s * r
            // s = dot(q - v1, r) / dot(r, r)
            Vector2 r = v2 - v1;
            float rr = Vector2.Dot(r, r);
            if (rr == 0.0f)
            {
                return false;
            }

            float s = Vector2.Dot(q - v1, r) / rr;
            if (s < 0.0f || 1.0f < s)
            {
                return false;
            }

            distance = t;

            return true; 
        }
    }
}
