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
        private float _distanceToGrabPoint = 0.4f;
        private Transform _playerTransform;
        private Transform _headTransform;
        private Transform _playerGrabPoint;
        private LayerMask _ledgeLayer;
        private LedgeGrabPoint _ledgeGrabPointInView;
        public LedgeGrabPoint LedgeGrabPointInView => _ledgeGrabPointInView;

        public LedgeGrabPoint AttachedLedgeGrabPoint;
        public Transform GrabPoint => _playerGrabPoint;

        public PlayerLedgeController(Transform playerTransform, Transform headTransform, Transform playerGrabPoint)
        {
            _playerTransform = playerTransform;
            _headTransform = headTransform;
            _playerGrabPoint = playerGrabPoint;
            _ledgeLayer = ~LayerMask.NameToLayer("Ledge");
        }

        public void HandleFindingLedge()
        {
            _ledgeGrabPointInView = DetectLedgeFromViewDirection();
        }

        public bool CanGrabToLedgeGrabPointInView()
        {
            if (_ledgeGrabPointInView.IsEmpty) return false;
            var distance = Vector3.Distance(_ledgeGrabPointInView.Position, _playerGrabPoint.transform.position);
            return distance < _distanceToGrabPoint;
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
            var ledge = viewHitInfo.transform.gameObject.GetComponent<Ledge>();
            return ledge.ProjectToGrabPoint(viewHitInfo.point);
        }
    }
}