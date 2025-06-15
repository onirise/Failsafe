using Failsafe.PlayerMovements;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace Failsafe.Player.Scripts.Interaction
{
    public class PhysicsInteraction : MonoBehaviour
    {
        [Header("Picking Up")]
        [SerializeField] private float _maxPickupDistance = 5f;
        
        [Header("Carrying")]
        [SerializeField] private float _carryingDistance = 2.5f;
        [SerializeField] private float _carrySpeed = 10f;
        [SerializeField] private LayerMask _carryingObjectLayer;
        
        [Header("Throwing")]
        [Tooltip("Данная сила умножается на число от 0 до 3 при зажатии кнопки броска.")]
        [SerializeField] private float _throwForce = 3f;
        [SerializeField] private float _throwTorqueForce = 3f;
        [Tooltip("[при зарядке броска] Линейное сокращение дистанции переноски с Carrying Distance до указанного значения.")]
        [SerializeField] private float _carryingDistanceShorteningTo = 0.8f;
        
        [Header("Additional Options")]
        [Tooltip("Данный вектор введёт для исправления бага, при котором на некоторых поверхностях объект застревал в текстуре." +
                 "Может быть удалён в будущем.")]
        [SerializeField] private Vector3 _grabHelperVector = new Vector3(0f,0.01f,0f);
        
        [Header("Debug")]
        [SerializeField] public GameObject _carryingObject;
        [SerializeField] public Rigidbody _carryingBody;
        [SerializeField] private Transform _playerCameraTransform;
        [SerializeField] [ReadOnly] private float _currentCarryingDistance;
        
        [SerializeField] private Vector3 _draggablePositionOffset;
        [SerializeField] private float _dragSpeed = 10f;
  

        private Quaternion _relativeRotation;
        
        [Inject]
        private InputHandler _inputHandler;
        
        private bool _isPreparingToThrow; 
        private float _throwForceMultiplier;
        private const float _maxForceMultiplier = 3f;
        
        private bool _allowToGrabOrDrop = true;
        private int _cachedCarryingLayer;
        

        [Header("Crosshair")] 
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private float _normalSize = 0.2f;
        [SerializeField] private float _hoverSize = 0.6f;
        [SerializeField] private float _scaleSpeed = 8f;    
        public bool IsDragging { get; private set; }

        private void Awake()
        {
            _currentCarryingDistance = _carryingDistance;

            if (!_playerCameraTransform)
            {
                Camera playerCamera = transform.root.GetComponentInChildren<Camera>();
                
                _playerCameraTransform = playerCamera.transform;
            }
            
        }

        private void Update()
        {
            UpdateCrosshairScale();
            if (_inputHandler.GrabOrDropTriggered && _allowToGrabOrDrop)
            {
                GrabOrDrop();
            }
            else if (!_inputHandler.GrabOrDropTriggered)
            {
                _allowToGrabOrDrop = true;
            }

            if (IsDragging)
            {
                if (!_carryingObject || !_carryingBody)
                {
                    DropItem();
                    return;
                }
                if (_inputHandler.AttackTriggered)
                {
                    _throwForceMultiplier = Mathf.Clamp(_throwForceMultiplier + Time.deltaTime, _throwForceMultiplier, _maxForceMultiplier);
                    _isPreparingToThrow = true;

                    if (_throwForceMultiplier < _maxForceMultiplier)
                    {
                        _currentCarryingDistance = Mathf.Lerp(_currentCarryingDistance, _carryingDistanceShorteningTo, Time.deltaTime);
                    }
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
            Vector3 targetPosition = _playerCameraTransform.position + _playerCameraTransform.forward * _currentCarryingDistance;
            
            Quaternion targetRotation = _playerCameraTransform.rotation * _relativeRotation;
            
            _carryingBody.linearVelocity = (targetPosition - _carryingBody.position) * _carrySpeed;
            _carryingBody.rotation = targetRotation;
            
            _carryingBody.angularVelocity = Vector3.zero;
        }
        
        private void GrabObject()
        {
            Physics.Raycast(
                _playerCameraTransform.position,
                _playerCameraTransform.forward,
                out RaycastHit hitInfo,
                _maxPickupDistance
            );

            if (!hitInfo.rigidbody)
                return;
            
            _carryingBody = hitInfo.rigidbody;
            _carryingBody.useGravity = false;
            
            _carryingObject = hitInfo.rigidbody.gameObject;
            
            _carryingObject.transform.parent = _playerCameraTransform;
            _relativeRotation = _carryingObject.transform.localRotation;
            _carryingObject.transform.parent = null;

            _carryingObject.transform.position += _grabHelperVector;
            
            _cachedCarryingLayer = _carryingObject.layer;
            
            _carryingObject.layer = _carryingObjectLayer.value >> 1;
            
            IsDragging = true;
            _isPreparingToThrow = false;
            _throwForceMultiplier = 0f;
        }

        public void ThrowObject(float throwForceMultiplier)
        {
            if (_carryingBody)
            {
                _carryingBody.useGravity = true;

                _carryingBody.AddForce(_playerCameraTransform.forward * (_throwForce * throwForceMultiplier),
                    ForceMode.Impulse);
                _carryingBody.AddTorque(_playerCameraTransform.forward * (_throwTorqueForce * throwForceMultiplier),
                    ForceMode.Impulse);
            }

            if (_carryingObject)  _carryingObject.layer = _cachedCarryingLayer;

            _carryingBody = null;
            _carryingObject = null;
            IsDragging = false;
            _isPreparingToThrow = false;
            _throwForceMultiplier = 0f;
            _currentCarryingDistance = _carryingDistance;
        }
        
        private void DropItem()
        {
            if (_carryingObject) _carryingObject.layer = _cachedCarryingLayer;
            if (_carryingBody) _carryingBody.useGravity = true;

            _carryingBody = null;
            _carryingObject = null;
            IsDragging = false;
            _currentCarryingDistance = _carryingDistance;
        }
        
        private void UpdateCrosshairScale()
        {
            float targetScale = _normalSize;

            if (!IsDragging)
            {
                Ray ray = new Ray(_playerCameraTransform.position, _playerCameraTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, _maxPickupDistance))
                {
                    if (hit.rigidbody != null)
                    {
                        targetScale = _hoverSize;
                    }
                }
            }

            float current = _crosshairImage.rectTransform.localScale.x;
            float next = Mathf.Lerp(current, targetScale, Time.deltaTime * _scaleSpeed);
            _crosshairImage.rectTransform.localScale = new Vector3(next, next, 1f);
        }
    }
}