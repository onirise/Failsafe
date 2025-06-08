using System.Collections.Generic;
using UnityEngine;

namespace Failsafe.Obstacles
{
    /// <summary>
    /// Выступ, за который можно зацепиться или взобраться
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Ledge : MonoBehaviour
    {
        public const float MinLedgeSize = 0.2f;
        public const float MaxLedgeSize = float.MaxValue;

        private float _distanceToLedge = 0.1f;

        // Если грань уступа упирается в стену/другой объект и игрок не должен за нее цепляться то отключить грань
        [Header("Какие грани будут активны для взаимодействия.")]
        [SerializeField] private bool _enableFront = true;
        [SerializeField] private bool _enableBack = true;
        [SerializeField] private bool _enableLeft = true;
        [SerializeField] private bool _enableRight = true;

        public LedgeEdge FrontEdge { get; private set; }
        public LedgeEdge BackEdge { get; private set; }
        public LedgeEdge LeftEdge { get; private set; }
        public LedgeEdge RightEdge { get; private set; }

        public LedgeEdge GetEdge(Vector3 direction)
        {
            if (direction == Vector3.forward) return FrontEdge;
            if (direction == Vector3.back) return BackEdge;
            if (direction == Vector3.left) return LeftEdge;
            if (direction == Vector3.right) return RightEdge;
            return null;
        }

        public void Awake()
        {
            InitializeEdges();
        }

        private void InitializeEdges()
        {
            Vector3 frontLeft = transform.TransformPoint(new Vector3(-0.5f, 0.5f, 0.5f));
            Vector3 frontRight = transform.TransformPoint(new Vector3(0.5f, 0.5f, 0.5f));
            Vector3 backLeft = transform.TransformPoint(new Vector3(-0.5f, 0.5f, -0.5f));
            Vector3 backRight = transform.TransformPoint(new Vector3(0.5f, 0.5f, -0.5f));
            FrontEdge = _enableFront ? new LedgeEdge(frontLeft, frontRight) : null;
            BackEdge = _enableBack ? new LedgeEdge(backLeft, backRight) : null;
            LeftEdge = _enableLeft ? new LedgeEdge(frontLeft, backLeft) : null;
            RightEdge = _enableRight ? new LedgeEdge(frontRight, backRight) : null;
        }

        public LedgeGrabPoint ProjectToGrabPoint(Vector3 point)
        {
            Vector3 localPoint = ToLocalPoint(point);
            var ledgeGrabPoint = ProjectToGrabPointLocal(localPoint);
            ToWorld(ref ledgeGrabPoint);
            Debug.DrawRay(ledgeGrabPoint.Position, ledgeGrabPoint.Normal, Color.black);
            return ledgeGrabPoint;
        }

        private void ToWorld(ref LedgeGrabPoint ledgeGrabPoint)
        {
            ledgeGrabPoint.Normal = transform.TransformDirection(ledgeGrabPoint.LocalNormal);
            ledgeGrabPoint.Position = ToWorldPoint(ledgeGrabPoint.LocalPosition);
            ledgeGrabPoint.Position -= ledgeGrabPoint.Normal * _distanceToLedge;
            if (ledgeGrabPoint.Transitions != null)
            {
                for (int i = 0; i < ledgeGrabPoint.Transitions.Count; i++)
                {
                    var transition = ledgeGrabPoint.Transitions[i];
                    transition.Direction = transform.TransformDirection(transition.LocalDirection);
                    if (!transition.Next.IsEmpty)
                    {
                        ToWorld(ref transition.Next);
                    }
                }
            }
        }

        private Vector3 ToLocalPoint(Vector3 point)
        {
            var localPoint = transform.InverseTransformPoint(point);
            localPoint.x *= transform.localScale.x;
            localPoint.y *= transform.localScale.y;
            localPoint.z *= transform.localScale.z;
            return localPoint;
        }

        private Vector3 ToWorldPoint(Vector3 localPoint)
        {
            localPoint.x /= transform.localScale.x;
            localPoint.y /= transform.localScale.y;
            localPoint.z /= transform.localScale.z;
            var point = transform.TransformPoint(localPoint);
            return point;
        }

        private LedgeGrabPoint ProjectToGrabPointLocal(Vector3 localPoint)
        {
            var result = new LedgeGrabPoint();
            result.Ledge = this;

            var maxX = transform.localScale.x / 2;
            var maxY = transform.localScale.y / 2;
            var maxZ = transform.localScale.z / 2;
            localPoint.y = maxY;
            localPoint.x = Mathf.Clamp(localPoint.x, -maxX, maxX);
            localPoint.z = Mathf.Clamp(localPoint.z, -maxZ, maxZ);
            result.LocalPosition = localPoint;

            var distanceToFront = Mathf.Abs(maxZ - localPoint.z);
            var distanceToBack = Mathf.Abs(-maxZ - localPoint.z);
            var distanceToLeft = Mathf.Abs(-maxX - localPoint.x);
            var distanceToRight = Mathf.Abs(maxX - localPoint.x);

            var signX = Mathf.Sign(localPoint.x);
            var signZ = Mathf.Sign(localPoint.z);
            var distX = maxX - Mathf.Abs(localPoint.x);
            var distZ = maxZ - Mathf.Abs(localPoint.z);

            if (!_enableFront && !_enableBack)
            {
                distZ = float.MaxValue;
            }
            else if (signZ > 0 && !_enableFront || signZ < 0 && !_enableBack)
            {
                distZ = maxZ * 2 - Mathf.Abs(localPoint.z);
                signZ *= -1;
            }
            if (!_enableLeft && !_enableRight)
            {
                distX = float.MaxValue;
            }
            else if (signX > 0 && !_enableRight || signX < 0 && !_enableLeft)
            {
                distX = maxX * 2 - Mathf.Abs(localPoint.x);
                signX *= -1;
            }

            if (distZ < distX)
            {
                result.LocalNormal = signZ * Vector3.forward;
                result.LocalPosition.z = signZ * maxZ;
                result.Edge = signZ > 0 ? FrontEdge : BackEdge;
                result.Width = signZ < 0 && _enableFront || signZ > 0 && _enableBack ? maxZ * 2 : float.MaxValue;
                distZ = 0;
            }
            else
            {
                result.LocalNormal = signX * Vector3.right;
                result.LocalPosition.x = signX * maxX;
                result.Edge = signX > 0 ? RightEdge : LeftEdge;
                result.Width = signX < 0 && _enableRight || signX > 0 && _enableLeft ? maxX * 2 : float.MaxValue;
                distX = 0;
            }

            var transitions = new List<LedgeGrabPointTransition>();
            if (distX == 0) // можем двигаться по оси Z
            {
                //Можем ли идти вперед
                if (distanceToFront <= _distanceToLedge && _enableFront)
                {
                    var transition = LedgeGrabPointTransition.OuterCorner(result, Vector3.forward);
                    transitions.Add(transition);
                }
                else if (distanceToFront > _distanceToLedge)
                {
                    var transition = LedgeGrabPointTransition.Straight(result, Vector3.forward, distanceToFront);
                    transitions.Add(transition);
                }
                //Можем ли идти назад
                if (distanceToBack <= _distanceToLedge && _enableBack)
                {
                    var transition = LedgeGrabPointTransition.OuterCorner(result, Vector3.back);
                    transitions.Add(transition);
                }
                else if (distanceToBack > _distanceToLedge)
                {
                    var transition = LedgeGrabPointTransition.Straight(result, Vector3.back, distanceToBack);
                    transitions.Add(transition);
                }
            }
            if (distZ == 0) // можем двигаться по оси X
            {
                //Можем ли идти вправо
                if (distanceToRight <= _distanceToLedge && _enableRight)
                {
                    var transition = LedgeGrabPointTransition.OuterCorner(result, Vector3.right);
                    transitions.Add(transition);
                }
                else if (distanceToRight > _distanceToLedge)
                {
                    var transition = LedgeGrabPointTransition.Straight(result, Vector3.right, distanceToRight);
                    transitions.Add(transition);
                }
                //Можем ли идти влево
                if (distanceToLeft <= _distanceToLedge && _enableLeft)
                {
                    var transition = LedgeGrabPointTransition.OuterCorner(result, Vector3.left);
                    transitions.Add(transition);
                }
                else if (distanceToLeft > _distanceToLedge)
                {
                    var transition = LedgeGrabPointTransition.Straight(result, Vector3.left, distanceToLeft);
                    transitions.Add(transition);
                }
            }
            result.Transitions = transitions;
            return result;
        }



        void OnValidate()
        {
            if (transform.localScale.x < MinLedgeSize || transform.localScale.y < MinLedgeSize || transform.localScale.z < MinLedgeSize)
            {
                transform.localScale = new Vector3(
                    Mathf.Clamp(transform.localScale.x, MinLedgeSize, MaxLedgeSize),
                    Mathf.Clamp(transform.localScale.y, MinLedgeSize, MaxLedgeSize),
                    Mathf.Clamp(transform.localScale.z, MinLedgeSize, MaxLedgeSize)
                );
            }
            if (!_enableFront && !_enableBack && !_enableLeft && !_enableRight)
            {
                _enableFront = true;
            }
        }

        void OnDrawGizmos()
        {
            var prevMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            Gizmos.color = Color.white;
            Vector3 frontRight = new Vector3(transform.localScale.x / 2 - _distanceToLedge, transform.localScale.y / 2, transform.localScale.z / 2 - _distanceToLedge);
            Vector3 frontLeft = new Vector3(-transform.localScale.x / 2 + _distanceToLedge, transform.localScale.y / 2, transform.localScale.z / 2 - _distanceToLedge);
            Vector3 backRight = new Vector3(transform.localScale.x / 2 - _distanceToLedge, transform.localScale.y / 2, -transform.localScale.z / 2 + _distanceToLedge);
            Vector3 backLeft = new Vector3(-transform.localScale.x / 2 + _distanceToLedge, transform.localScale.y / 2, -transform.localScale.z / 2 + _distanceToLedge);
            if (_enableFront)
            {
                Gizmos.DrawLine(frontRight, frontLeft);
            }
            if (_enableBack)
            {
                Gizmos.DrawLine(backRight, backLeft);
            }
            if (_enableLeft)
            {
                Gizmos.DrawLine(frontLeft, backLeft);
            }
            if (_enableRight)
            {
                Gizmos.DrawLine(frontRight, backRight);
            }
            Gizmos.matrix = prevMatrix;
        }
    }

    /// <summary>
    /// Грань выступа
    /// </summary>
    public class LedgeEdge
    {
        public const float MaxDistance = 3f;
        /// <summary>
        /// Свободное расстояние снизу
        /// </summary>
        public float DownDistance { get; private set; }
        /// <summary>
        /// Свободное расстояние сверху
        /// </summary>
        public float UpDistance { get; private set; }
        public Vector3 Point1 { get; private set; }
        public Vector3 Point2 { get; private set; }
        public Vector3 Middle { get; private set; }
        private static int _ledgeLayer = LayerMask.NameToLayer("Ledge");

        public LedgeEdge(Vector3 point1, Vector3 point2)
        {
            Point1 = point1;
            Point2 = point2;
            Middle = Vector3.Lerp(Point1, Point2, 0.5f);
            CalculateHeight();
        }

        private void CalculateHeight()
        {

            if (Physics.Raycast(Middle, Vector3.down, out var downHitInfo, MaxDistance, _ledgeLayer))
            {
                DownDistance = Mathf.Abs(downHitInfo.point.y - Middle.y);
                Debug.DrawRay(Middle, Vector3.down * DownDistance, Color.black, 5);
            }
            else
            {
                DownDistance = MaxDistance;
                Debug.DrawRay(Middle, Vector3.down * MaxDistance, Color.gray, 5);
            }
            if (Physics.Raycast(Middle, Vector3.up, out var upHitInfo, MaxDistance, _ledgeLayer))
            {
                UpDistance = Mathf.Abs(upHitInfo.point.y - Middle.y);
            }
            else
            {
                UpDistance = MaxDistance;
            }
        }
    }
}