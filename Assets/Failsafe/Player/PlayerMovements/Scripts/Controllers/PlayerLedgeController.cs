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

        private PlayerMovementParameters _movementParameters;

        private Transform _playerTransform;
        private Transform _headTransform;
        private Transform _playerGrabPoint;
        private LayerMask _ledgeLayer;
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
            _ledgeLayer = ~LayerMask.NameToLayer("Ledge");
            _movementParameters = movementParameters;
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
                && _ledgeGrabPointInView.Edge.DownDistance > _movementParameters.GrabLedgeMinHeight;
        }

        public bool CanClimbOverLedge()
        {
            if (_distanceToLedgeGrabPointInFrontBottom < 0) return false;
            if (_ledgeGrabPointInFrontBottom.IsEmpty) return false;

            return _distanceToLedgeGrabPointInFrontBottom <= _movementParameters.ClimbOverMaxDistanceToLedge
                && _ledgeGrabPointInFrontBottom.Width <= _movementParameters.ClimbOverLedgeMaxWidth
                && _ledgeGrabPointInFrontBottom.Edge.DownDistance <= _movementParameters.ClimbOverLedgeMaxHeight;
        }

        public bool CanClimbOnLedge()
        {
            if (_distanceToLedgeGrabPointInFrontBottom < 0) return false;
            if (_ledgeGrabPointInFrontBottom.IsEmpty) return false;

            return _distanceToLedgeGrabPointInFrontBottom <= _movementParameters.ClimbOnMaxDistanceToLedge
                && _ledgeGrabPointInFrontBottom.Width > _movementParameters.ClimbOverLedgeMaxWidth
                && _ledgeGrabPointInFrontBottom.Edge.DownDistance <= _movementParameters.ClimbOnLedgeMaxHeight;
        }

        private LedgeGrabPoint DetectLedgeFromViewDirection()
        {
            Debug.DrawRay(_headTransform.position, _headTransform.forward * _ledgeFindDistance, Color.white);
            if (!Physics.SphereCast(_headTransform.position, _forwardSpereRadius, _headTransform.forward, out var ledgeHitInfo, _ledgeFindDistance, _ledgeLayer))
            {
                return LedgeGrabPoint.Empty;
            }
            if (Physics.SphereCast(_headTransform.position, _forwardSpereRadius, _headTransform.forward, out var viewHitInfo, _ledgeFindDistance))
            {
                if (viewHitInfo.collider.gameObject != ledgeHitInfo.collider.gameObject)
                {
                    return LedgeGrabPoint.Empty;
                }
            }
            if (viewHitInfo.transform.gameObject.TryGetComponent<Ledge>(out var ledge))
                return ledge.ProjectToGrabPoint(viewHitInfo.point);
            return LedgeGrabPoint.Empty;
        }

        private LedgeGrabPoint DetectLedgeFromForwardDirection(Vector3 originPosition, ref float distance)
        {
            Debug.DrawRay(originPosition, _playerTransform.forward * _ledgeFindDistance, Color.grey);
            if (!Physics.SphereCast(originPosition, _forwardSpereRadius, _playerTransform.forward, out var ledgeHitInfo, _ledgeFindDistance, _ledgeLayer))
            {
                distance = -1;
                return LedgeGrabPoint.Empty;
            }
            if (Physics.SphereCast(originPosition, _forwardSpereRadius, _playerTransform.forward, out var viewHitInfo, _ledgeFindDistance))
            {
                if (viewHitInfo.collider.gameObject != ledgeHitInfo.collider.gameObject)
                {
                    Debug.Log($"NotSameObject {viewHitInfo.collider.gameObject.name} {ledgeHitInfo.collider.gameObject.name} ");
                    distance = -1;
                    return LedgeGrabPoint.Empty;
                }
            }
            return viewHitInfo.transform.gameObject.TryGetComponent(out Ledge ledge)
                ? ledge.ProjectToGrabPoint(viewHitInfo.point)
                : LedgeGrabPoint.Empty;
        }
    }
}