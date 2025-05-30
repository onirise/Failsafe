using Failsafe.PlayerMovements;
using System;
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
        
        private PlayerController _playerController;
        
        private bool _isPreparingToThrow;
        [SerializeField] private float _throwForceMultiplier;
        private const float _maxForceMultiplier = 3f;
        
        private bool _allowToGrabOrDrop = true;
        
        public bool IsDragging { get; private set; }

        private void Awake()
        {
            if (!_playerCameraTransform)
            {
                Camera playerCamera = transform.root.GetComponentInChildren<Camera>();
                
                _playerCameraTransform = playerCamera.transform;
            }
            
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (_playerController.InputHandler == null)
            {
                throw new Exception("InputHandler is not set.");
            }
            
            if (_playerController.InputHandler.GrabOrDropTriggered && _allowToGrabOrDrop)
            {
                GrabOrDrop();
            }
            else if (!_playerController.InputHandler.GrabOrDropTriggered)
            {
                _allowToGrabOrDrop = true;
            }

            if (IsDragging)
            {
                if (_playerController.InputHandler.AttackTriggered)
                {
                    _throwForceMultiplier = Mathf.Clamp(_throwForceMultiplier + Time.deltaTime, _throwForceMultiplier, _maxForceMultiplier);
                    _isPreparingToThrow = true;
                }
                else if (_isPreparingToThrow)
                {
                    ThrowObject(_throwForceMultiplier);
                }
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
            _allowToGrabOrDrop = false;

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
            _isPreparingToThrow = false;
            _throwForceMultiplier = 0f;
        }

        public void ThrowObject(float throwForceMultiplier)
        {
            _carryingBody.useGravity = true;
            
            _carryingBody.AddForce(_playerCameraTransform.forward * (_throwForce * throwForceMultiplier), ForceMode.Impulse);
            
            _carryingBody = null;
            _carryingObject = null;
            IsDragging = false;
            _isPreparingToThrow = false;
            _throwForceMultiplier = 0f;
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