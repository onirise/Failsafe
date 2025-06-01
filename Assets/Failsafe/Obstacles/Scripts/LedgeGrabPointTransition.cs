using UnityEngine;

namespace Failsafe.Obstacles
{
    /// <summary>
    /// Возможный переход из одной точки в другую
    /// </summary>
    public class LedgeGrabPointTransition
    {
        public const float MaxDistance = 1f;
        public enum TransidiotnType { Straight, OuterCorner }
        public TransidiotnType Type;
        public Vector3 LocalDirection;
        public Vector3 Direction;
        public float Distance;
        public LedgeGrabPoint Current;
        public LedgeGrabPoint Next;

        public static LedgeGrabPointTransition OuterCorner(LedgeGrabPoint current, Vector3 localDirection)
        {
            var next = current;
            next.LocalNormal = localDirection;
            next.Edge = current.Ledge.GetEdge(localDirection);
            return new LedgeGrabPointTransition
            {
                Type = TransidiotnType.OuterCorner,
                Distance = 0f,
                LocalDirection = localDirection,
                Current = current,
                Next = next
            };
        }

        public static LedgeGrabPointTransition Straight(LedgeGrabPoint current, Vector3 localDirection, float distance)
        {
            distance = Mathf.Min(distance, MaxDistance);
            var next = current;
            next.LocalPosition += localDirection * distance;
            return new LedgeGrabPointTransition
            {
                Type = TransidiotnType.Straight,
                Distance = distance,
                LocalDirection = localDirection,
                Current = current,
                Next = next
            };
        }

        public override string ToString()
        {
            return $"{Type} {Direction}";
        }
    }
}