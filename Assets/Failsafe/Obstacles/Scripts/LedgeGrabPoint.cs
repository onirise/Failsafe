using System.Collections.Generic;
using UnityEngine;

namespace Failsafe.Obstacles
{
    /// <summary>
    /// Точка на уступе, за которую можно ухватиться
    /// </summary>
    public struct LedgeGrabPoint
    {
        public Ledge Ledge;
        public LedgeEdge Edge;
        public Vector3 LocalPosition;
        public Vector3 Position;
        public Vector3 LocalNormal;
        public Vector3 Normal;
        public float Width;
        public List<LedgeGrabPointTransition> Transitions;

        public bool IsEmpty => LocalNormal == Vector3.zero;
        public static LedgeGrabPoint Empty => new LedgeGrabPoint();

        public override string ToString()
        {
            var transitionsStiring = Transitions != null ? string.Join(",", Transitions) : string.Empty;
            return $"{Position}; {Normal}; Transitions: [{transitionsStiring}]";
        }
    }
}