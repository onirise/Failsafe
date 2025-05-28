using UnityEngine;

namespace Failsafe.Player.Interaction
{
    public class PhysicsInteraction : MonoBehaviour
    {
        [SerializeField] private float _carryingDistance = 2.5f;
        [SerializeField] private float _maxPickupDistance = 5f;
        
        [SerializeField] private GameObject _carryingObject;
        [SerializeField] private Rigidbody _carryingBody;
        [SerializeField] private Transform _playerCameraTransform;
        
        [SerializeField] private Vector3 _draggablePositionOffset;
        [SerializeField] private float _dragSpeed = 10f;
        
        [Tooltip("Данная сила умножается на число от 1 до 3 при зажатии кнопки броска.")]
        [SerializeField] private float _throwForce = 3f;

        private Quaternion _relativeRotation;
        
        public bool IsDragging { get; private set; }

        private void Awake()
        {
            if (!_playerCameraTransform)
            {
                Camera playerCamera = transform.root.GetComponentInChildren<Camera>();
                
                _playerCameraTransform = playerCamera.transform;
            }
        }
        
        private void FixedUpdate()
        {
            if (_carryingObject)
            {
                DragObject();
            }
        }

        public void GrabOrDrop()
        {
            if (!_carryingObject)
            {
                GrabObject();
            }
            else
            {
                DropItem();
            }
        }
        
        private void DragObject()
        {
            Vector3 targetPosition = transform.position + _playerCameraTransform.forward * _carryingDistance;
            Quaternion targetRotation = transform.rotation * _relativeRotation;
            
            _carryingBody.linearVelocity = (targetPosition - _carryingBody.position + _draggablePositionOffset) * _dragSpeed;
            
            _carryingBody.rotation = targetRotation;
            
            _carryingBody.angularVelocity = Vector3.zero;
        }
        
        private void GrabObject()
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _maxPickupDistance);

            if (!hitInfo.rigidbody)
                return;
            
            _carryingBody = hitInfo.rigidbody;
            _carryingBody.useGravity = false;
            
            _carryingObject = hitInfo.rigidbody.gameObject;
            
            _carryingObject.transform.parent = transform;
            _relativeRotation = _carryingObject.transform.localRotation;
            _carryingObject.transform.parent = null;
            
            IsDragging = true;
        }

        public void ThrowObject(float throwForceMultiplier)
        {
            _carryingBody.useGravity = true;
            
            _carryingBody.AddForce(_playerCameraTransform.forward * (_throwForce * throwForceMultiplier), ForceMode.Impulse);
            
            _carryingBody = null;
            _carryingObject = null;
            IsDragging = false;
        }
        
        private void DropItem()
        {
            _carryingBody.useGravity = true;
            _carryingBody = null;
            _carryingObject = null;
            IsDragging = false;
        }
    }
}