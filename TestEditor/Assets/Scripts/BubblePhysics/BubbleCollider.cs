using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public abstract class BubbleCollider
    {
        public virtual bool RayCast(BubbleRay ray, out float distance)
        {
            distance = float.MaxValue;
            return false;
        }
    }
}
