using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Управляет вращением камеры (головы) и модели (тела) игрока
    /// </summary>
    public class PlayerRotationController
    {
        private readonly Transform _headTransform;
        private readonly Transform _playerTransform;
        private readonly InputHandler _inputHandler;
        private float _cameraVerticalRotation = 0f;
        private float _cameraHorizontalRotation = 0f;
        private float _mouseSensitivity = 10f;
        private bool _syncBodyRotationWithHead = true;

        public PlayerRotationController(Transform playerTransform, Transform headTransform, InputHandler inputHandler)
        {
            _playerTransform = playerTransform;
            _headTransform = headTransform;
            _inputHandler = inputHandler;
        }

        /// <summary>
        /// Синхронизировать поворт игрока к повороту головы
        /// </summary>
        public void SyncBodyRotationToHead()
        {
            _syncBodyRotationWithHead = true;
            _cameraHorizontalRotation = 0;
        }

        /// <summary>
        /// Обработать вращение тела и головы игрока
        /// </summary>
        public void HandlePlayerRotation()
        {
            var rotation = _inputHandler.RotationInput * _mouseSensitivity * Time.deltaTime;
            _cameraVerticalRotation += rotation.y;
            _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -85f, 85f);
            if (_syncBodyRotationWithHead)
            {
                _headTransform.transform.localRotation = Quaternion.Euler(-_cameraVerticalRotation, 0, 0);
                _playerTransform.Rotate(Vector3.up * rotation.x);
            }
            else
            {
                _cameraHorizontalRotation += rotation.x;
                _cameraHorizontalRotation = Mathf.Clamp(_cameraHorizontalRotation, -120f, 120f);
                _headTransform.transform.localRotation = Quaternion.Euler(-_cameraVerticalRotation, _cameraHorizontalRotation, 0);
            }
        }

        /// <summary>
        /// Повернуть модель игрока к нужному направлению. Отключает синхронизацию поворота тела к голове
        /// </summary>
        /// <param name="targetDirection"></param>
        public void RotateBodyToDirection(Vector3 targetDirection)
        {
            _syncBodyRotationWithHead = false;
            var yAlignedNormal = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
            var targetRotation = Quaternion.LookRotation(yAlignedNormal, _playerTransform.up);
            var headRotation = _headTransform.rotation;
            _playerTransform.rotation = targetRotation;
            _headTransform.rotation = headRotation;
        }

        /// <summary>
        /// Повернуть тело в сторону головы
        /// </summary>
        public void RotateBodyToHead()
        {
            var targetDirection = _headTransform.forward;
            var yAlignedNormal = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
            var targetRotation = Quaternion.LookRotation(yAlignedNormal, _playerTransform.up);
            _playerTransform.rotation = targetRotation;
            _headTransform.localRotation = Quaternion.Euler(-_cameraVerticalRotation, 0, 0);
        }

        /// <summary>
        /// Повернуть голову в направлении тела
        /// </summary>
        public void RotateHeadToBody()
        {
            _headTransform.localRotation = Quaternion.Euler(-_cameraVerticalRotation, 0, 0);
        }
    }
}