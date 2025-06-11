using Failsafe.Obstacles;
using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Контроллер уступов
    /// </summary>
    public class PlayerLedgeController
    {
        private float _ledgeFindDistance = 3f;
        private float _forwardSpereRadius = 0.2f;
        private float _bottomFrontRayHeight = 0.6f;
        /// <summary>
        /// Угол в градусах между нормалью уступа и направлением игрока
        /// Нужно чтобы игрок смотрел под определенным углом на уступ чтобы на него забраться
        /// </summary>
        private float _angleToLedgeNormalInDegree = 60;
        /// <summary>
        /// Минус косинус угла <seealso cref="_angleToLedgeNormalInDegree"/>. Используется для оптимизации определения угла по Vector3.Dot
        /// </summary>
        private readonly float _angleToLedgeNormal;

        private PlayerMovementParameters _movementParameters;

        private Transform _playerTransform;
        private Transform _headTransform;
        private Transform _playerGrabPoint;
        private int _ledgeMask;
        private LedgeGrabPoint _ledgeGrabPointInView;
        private LedgeGrabPoint _ledgeGrabPointInFrontBottom;
        private float _distanceToLedgeGrabPointInFrontBottom;

        public LedgeGrabPoint LedgeGrabPointInView => _ledgeGrabPointInView;
        public LedgeGrabPoint LedgeGrabPointInFrontBottom => _ledgeGrabPointInFrontBottom;
        public LedgeGrabPoint AttachedLedgeGrabPoint;
        public Transform GrabPoint => _playerGrabPoint;

        public PlayerLedgeController(Transform playerTransform, Transform headTransform, Transform playerGrabPoint, PlayerMovementParameters movementParameters)
        {
            _playerTransform = playerTransform;
            _headTransform = headTransform;
            _playerGrabPoint = playerGrabPoint;
            _ledgeMask = LayerMask.GetMask("Ledge");
            _movementParameters = movementParameters;
            _angleToLedgeNormal = -Mathf.Cos(_angleToLedgeNormalInDegree * Mathf.Deg2Rad);
        }

        public void HandleFindingLedge()
        {
            _ledgeGrabPointInView = DetectLedgeFromViewDirection();
            _ledgeGrabPointInFrontBottom = DetectLedgeFromForwardDirection(_playerTransform.position + Vector3.up * _bottomFrontRayHeight, ref _distanceToLedgeGrabPointInFrontBottom);
        }

        public bool CanGrabToLedgeGrabPointInView()
        {
            if (_ledgeGrabPointInView.IsEmpty) return false;
            var distance = Vector3.Distance(_ledgeGrabPointInView.Position, _playerGrabPoint.transform.position);
            return distance < _movementParameters.GrabLedgeMaxDistance
                && _ledgeGrabPointInView.Edge.DownDistance > _movementParameters.GrabLedgeMinHeight
                && Vector3.Dot(_ledgeGrabPointInView.Normal, _playerTransform.forward) <= _angleToLedgeNormal;
        }

        public bool CanClimbOverLedge()
        {
            if (_distanceToLedgeGrabPointInFrontBottom < 0) return false;
            if (_ledgeGrabPointInFrontBottom.IsEmpty) return false;

            return _distanceToLedgeGrabPointInFrontBottom <= _movementParameters.ClimbOverMaxDistanceToLedge
                && _ledgeGrabPointInFrontBottom.Width <= _movementParameters.ClimbOverLedgeMaxWidth
                && _ledgeGrabPointInFrontBottom.Edge.DownDistance <= _movementParameters.ClimbOverLedgeMaxHeight
                && Vector3.Dot(_ledgeGrabPointInFrontBottom.Normal, _playerTransform.forward) <= _angleToLedgeNormal;
        }

        public bool CanClimbOnLedge()
        {
            if (_distanceToLedgeGrabPointInFrontBottom < 0) return false;
            if (_ledgeGrabPointInFrontBottom.IsEmpty) return false;

            return _distanceToLedgeGrabPointInFrontBottom <= _movementParameters.ClimbOnMaxDistanceToLedge
                && _ledgeGrabPointInFrontBottom.Width > _movementParameters.ClimbOverLedgeMaxWidth
                && _ledgeGrabPointInFrontBottom.Edge.DownDistance <= _movementParameters.ClimbOnLedgeMaxHeight
                && Vector3.Dot(_ledgeGrabPointInFrontBottom.Normal, _playerTransform.forward) <= _angleToLedgeNormal;
        }

        private LedgeGrabPoint DetectLedgeFromViewDirection()
        {
            Debug.DrawRay(_headTransform.position, _headTransform.forward * _ledgeFindDistance, Color.white);
            if (!Physics.SphereCast(_headTransform.position, _forwardSpereRadius, _headTransform.forward, out var ledgeHitInfo, _ledgeFindDistance, _ledgeMask))
            {
                return LedgeGrabPoint.Empty;
            }
            return CheckLedgeCandidate(_headTransform.position, _headTransform.forward, out _, ledgeHitInfo);
        }

        private LedgeGrabPoint DetectLedgeFromForwardDirection(Vector3 originPosition, ref float distance)
        {
            Debug.DrawRay(originPosition, _playerTransform.forward * _ledgeFindDistance, Color.grey);
            if (!Physics.SphereCast(originPosition, _forwardSpereRadius, _playerTransform.forward, out var ledgeHitInfo, _ledgeFindDistance, _ledgeMask))
            {
                distance = -1;
                return LedgeGrabPoint.Empty;
            }
            return CheckLedgeCandidate(originPosition, _playerTransform.forward, out distance, ledgeHitInfo);
        }

        private LedgeGrabPoint CheckLedgeCandidate(Vector3 originPosition, Vector3 direction, out float distance, RaycastHit ledgeHitInfo)
        {
            if (Physics.SphereCast(originPosition, _forwardSpereRadius, direction, out var viewHitInfo, _ledgeFindDistance))
            {
                if (viewHitInfo.collider.gameObject != ledgeHitInfo.collider.gameObject)
                {
                    Debug.Log($"Not same objects viewHitInfo: {viewHitInfo.collider.gameObject.name} ledgeHitInfo: {ledgeHitInfo.collider.gameObject.name}");
                    distance = -1;
                    return LedgeGrabPoint.Empty;
                }
            }
            if (!viewHitInfo.transform.gameObject.TryGetComponent<Ledge>(out var ledge))
            {
                Debug.Log("Ledge component not exists");
                distance = -1;
                return LedgeGrabPoint.Empty;
            }

            distance = viewHitInfo.distance;
            return ledge.ProjectToGrabPoint(viewHitInfo.point);
        }
    }
}