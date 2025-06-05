using System.Collections;
using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Контроллер тела персонажа
    /// </summary>
    public class PlayerBodyController
    {
        private readonly Transform _headTransform;
        private readonly CharacterController _characterController;
        // Пока это капсула, позже будет модель игрока котороя должна менять высоту с помощью анимации. 
        // Удалить после исправления
        private readonly Transform _playerBody;
        // Нужен только для запуска корутины, позже камера будет привязана к голове персонажа и двигаться анимацией приседания. 
        // Удалить после исправления
        private readonly MonoBehaviour _monoBehaviour;

        private Vector3 _standingHeadPosition;
        private float _standingCCCenterHeight = 1f;
        private float _standingCCHeight = 2f;
        private float _standingCCStepOffset = 1f;
        private float _standingBodyHeight = 1f;
        private float _standingBodyCenterHeight = 1f;

        private Vector3 _crouchingHeadPosition;
        private float _crouchingCCCenterHeight = 0.5f;
        private float _crouchingCCHeight = 1f;
        private float _crouchingCCStepOffset = 0.2f;
        private float _crouchingBodyHeight = 0.5f;
        private float _crouchingBodyCenterHeight = 0.5f;

        private Vector3 _slideHeadPosition;
        private float _elapsedTime;
        private float _lerpHeadTime = 0.5f;
        private Coroutine _lerpHeadCoroutine;
        private int _ignoreLedgeLayer;

        public PlayerBodyController(Transform headTransform, CharacterController characterController, Transform playerBody, MonoBehaviour monoBehaviour)
        {
            _headTransform = headTransform;
            _characterController = characterController;
            _playerBody = playerBody;
            _standingHeadPosition = _headTransform.localPosition;
            _crouchingHeadPosition = _standingHeadPosition + Vector3.down * (_standingHeadPosition.y * 0.5f);
            _slideHeadPosition = _standingHeadPosition + Vector3.down * (_standingHeadPosition.y * 0.7f);
            _monoBehaviour = monoBehaviour;
            _ignoreLedgeLayer = LayerMask.NameToLayer("Ledge");
        }

        public bool CanStand()
        {
            // Точка слегка выше обычной капсулы, чтобы случайно не задеть пол
            var point = _characterController.transform.position + Vector3.up * 0.51f;
            var sphereRadius = _characterController.radius - _characterController.skinWidth;
            var distance = _standingCCHeight - sphereRadius * 2;
            if (Physics.SphereCast(point, sphereRadius, Vector3.up, out var hitInfo, distance, _ignoreLedgeLayer))
            {
                Debug.Log("Cant stand :" + hitInfo.transform.name);
                return false;
            }
            return true;
        }

        public void Stand()
        {
            _characterController.center = _standingCCCenterHeight * Vector3.up;
            _characterController.height = _standingCCHeight;
            _characterController.stepOffset = _standingCCStepOffset;

            _playerBody.localPosition = _standingBodyCenterHeight * Vector3.up;
            _playerBody.localScale = new Vector3(1, _standingBodyHeight, 1);

            if (_lerpHeadCoroutine != null)
                _monoBehaviour.StopCoroutine(_lerpHeadCoroutine);
            _lerpHeadCoroutine = _monoBehaviour.StartCoroutine(ChangeHeadPositionCoroutine(_standingHeadPosition));
        }

        public void Crouch()
        {
            _characterController.center = _crouchingCCCenterHeight * Vector3.up;
            _characterController.height = _crouchingCCHeight;
            _characterController.stepOffset = _crouchingCCStepOffset;

            _playerBody.localPosition = _crouchingBodyCenterHeight * Vector3.up;
            _playerBody.localScale = new Vector3(1, _crouchingBodyHeight, 1);

            if (_lerpHeadCoroutine != null)
                _monoBehaviour.StopCoroutine(_lerpHeadCoroutine);
            _lerpHeadCoroutine = _monoBehaviour.StartCoroutine(ChangeHeadPositionCoroutine(_crouchingHeadPosition));
        }

        public void Slide()
        {
            // Высота модели в скольжении такая же как в присяди, 
            // чтобы игрок не мог куда то проскользить и там застрять без возможности присесть
            _characterController.center = _crouchingCCCenterHeight * Vector3.up;
            _characterController.height = _crouchingCCHeight;
            _characterController.stepOffset = _crouchingCCStepOffset;

            _playerBody.localPosition = _crouchingBodyCenterHeight * Vector3.up;
            _playerBody.localScale = new Vector3(1, _crouchingBodyHeight, 1);

            if (_lerpHeadCoroutine != null)
                _monoBehaviour.StopCoroutine(_lerpHeadCoroutine);
            _lerpHeadCoroutine = _monoBehaviour.StartCoroutine(ChangeHeadPositionCoroutine(_slideHeadPosition));
        }

        // Плавно опускается только голова, а не все тело, чтобы не поломать логику перемещения
        private IEnumerator ChangeHeadPositionCoroutine(Vector3 targetPosition)
        {
            _elapsedTime = 0;
            while (_elapsedTime < _lerpHeadTime)
            {
                _headTransform.localPosition = Vector3.Lerp(_headTransform.localPosition, targetPosition, _elapsedTime / _lerpHeadTime);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            _headTransform.localPosition = targetPosition;
            yield return null;
        }
    }
}