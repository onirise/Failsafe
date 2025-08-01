using Failsafe.Player.Model;
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
        [Inject] PlayerModelParameters _playerModelParameters;
        [Header("Picking Up")]
        [SerializeField] private float _maxPickupDistance = 5f;

        [Header("Carrying")]
        [SerializeField] private float _carryingDistance = 2.5f;
        [SerializeField] private float _carrySpeed = 10f;
        [SerializeField] private LayerMask _carryingObjectLayer;

        [Header("Throwing")]
        //[Tooltip("Данная сила умножается на число от 0 до 3 при зажатии кнопки броска.")]
        //[SerializeField] private float _throwForce = 3f;
        //[SerializeField] private float _throwTorqueForce = 3f;
        [Tooltip("[при зарядке броска] Линейное сокращение дистанции переноски с Carrying Distance до указанного значения.")]
        [SerializeField] private float _carryingDistanceShorteningTo = 0.8f;

        [Header("Additional Options")]
        [Tooltip("Данный вектор введёт для исправления бага, при котором на некоторых поверхностях объект застревал в текстуре." +
                 "Может быть удалён в будущем.")]
        [SerializeField] private Vector3 _grabHelperVector = new Vector3(0f, 0.01f, 0f);

        [Header("Debug")]
        [SerializeField] public GameObject CarryingObject;
        [SerializeField] public Rigidbody CarryingBody;
        [SerializeField] private Transform _playerCameraTransform;
        [SerializeField][ReadOnly] private float _currentCarryingDistance;

        [SerializeField] private Vector3 _draggablePositionOffset;
        [SerializeField] private float _dragSpeed = 10f;


        private Quaternion _relativeRotation;

        [Inject]
        private InputHandler _inputHandler;
        [Inject] private PlayerHandsContainer _playerHandsContainer;

        private bool _isPreparingToThrow;
        private float _throwForceMultiplier;
        private const float _maxForceMultiplier = 3f;

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
            if (_inputHandler.GrabOrDropAction.WasPressedThisFrame())
            {
                GrabOrDrop();
            }

            if (IsDragging)
            {
                if (!CarryingObject || !CarryingBody)
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
            if (CarryingObject)
            {
                DragObject();
            }
        }

        public void GrabOrDrop()
        {

            if (!CarryingObject)
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

            CarryingBody.linearVelocity = (targetPosition - CarryingBody.position) * _carrySpeed;
            CarryingBody.rotation = targetRotation;

            CarryingBody.angularVelocity = Vector3.zero;
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

            if (hitInfo.transform.TryGetComponent<Item>(out var itemObject))
            {
                if (_playerHandsContainer.State == PlayerHandsContainer.HandState.ItemInHand)
                {
                    _playerHandsContainer.DropItemFromHand();
                }
                _playerHandsContainer.TryTakeItemInHand(itemObject);
                return;
            }

            CarryingBody = hitInfo.rigidbody;
            CarryingBody.useGravity = false;

            CarryingObject = hitInfo.rigidbody.gameObject;

            CarryingObject.transform.parent = _playerCameraTransform;
            _relativeRotation = CarryingObject.transform.localRotation;
            CarryingObject.transform.parent = null;

            CarryingObject.transform.position += _grabHelperVector;

            _cachedCarryingLayer = CarryingObject.layer;

            CarryingObject.layer = _carryingObjectLayer.value >> 1;

            IsDragging = true;
            _isPreparingToThrow = false;
            _throwForceMultiplier = 0f;
        }

        public void ThrowObject(float throwForceMultiplier)
        {
            if (CarryingBody)
            {
                CarryingBody.useGravity = true;

                CarryingBody.AddForce(_playerCameraTransform.forward * (_playerModelParameters.ThrowPower * throwForceMultiplier),
                    ForceMode.Impulse);
                CarryingBody.AddTorque(_playerCameraTransform.forward * (_playerModelParameters.ThrowTorquePower * throwForceMultiplier),
                    ForceMode.Impulse);
            }

            if (CarryingObject) CarryingObject.layer = _cachedCarryingLayer;

            CarryingBody = null;
            CarryingObject = null;
            IsDragging = false;
            _isPreparingToThrow = false;
            _throwForceMultiplier = 0f;
            _currentCarryingDistance = _carryingDistance;
        }

        private void DropItem()
        {
            if (CarryingObject) CarryingObject.layer = _cachedCarryingLayer;
            if (CarryingBody) CarryingBody.useGravity = true;

            CarryingBody = null;
            CarryingObject = null;
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