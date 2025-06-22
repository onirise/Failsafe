using Failsafe.Obstacles;
using Failsafe.PlayerMovements.Controllers;
using System.Linq;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Зацепление за выступ
    /// </summary>
    public class GrabLedgeState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerMovementParameters _movementParameters;
        private readonly PlayerRotationController _playerRotationController;
        private readonly PlayerLedgeController _playerLedgeController;
        private Ledge _ledge;
        private float _minDuration = 0.5f;
        private float _stateProgress = 0f;
        private float _directionAngleThreathfold = 10f;

        public GrabLedgeState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementController movementController,
            PlayerMovementParameters movementParameters,
            PlayerRotationController playerRotationController,
            PlayerLedgeController playerLedgeController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementController = movementController;
            _movementParameters = movementParameters;
            _playerRotationController = playerRotationController;
            _playerLedgeController = playerLedgeController;
        }

        public bool CanFinish() => _stateProgress >= _minDuration;

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(GrabLedgeState));
            _stateProgress = 0;
            _playerLedgeController.AttachedLedgeGrabPoint = _playerLedgeController.LedgeGrabPointInView;
            _ledge = _playerLedgeController.AttachedLedgeGrabPoint.Ledge;
            _playerRotationController.RotateBodyToDirection(-_playerLedgeController.AttachedLedgeGrabPoint.Normal);
            StickToObstacle(_playerLedgeController.AttachedLedgeGrabPoint);
            _characterController.SimpleMove(Vector3.zero);
            _movementController.Move(Vector3.zero);
            _movementController.SetGravity(Vector3.zero);
            //TODO: переделать все перемещение на PlayerMovementController
        }

        public override void Update()
        {
            _stateProgress += Time.deltaTime;
            var movementInput = _inputHandler.MovementInput;
            _playerLedgeController.AttachedLedgeGrabPoint = _ledge.ProjectToGrabPoint(_characterController.transform.position);
            StickToObstacle(_playerLedgeController.AttachedLedgeGrabPoint);
            if (movementInput.x != 0)
            {
                var movementX = movementInput.x;
                var movementDirection = _characterController.transform.right * Mathf.Sign(movementX);
                var transition = _playerLedgeController.AttachedLedgeGrabPoint.Transitions.FirstOrDefault(x => Vector3.Angle(x.Direction, movementDirection) < _directionAngleThreathfold);
                if (transition != null && transition.Type == LedgeGrabPointTransition.TransidiotnType.Straight)
                {
                    var movement = _movementParameters.GrabLedgeSpeed * movementX * Time.deltaTime * _characterController.transform.right;
                    _characterController.Move(movement);
                    _playerRotationController.RotateBodyToDirection(-_playerLedgeController.AttachedLedgeGrabPoint.Normal);
                }
                if (transition != null && transition.Type == LedgeGrabPointTransition.TransidiotnType.OuterCorner)
                {
                    if (transition.Next.Edge.DownDistance > 2f)
                    {
                        StickToObstacle(transition.Next);
                        _playerRotationController.RotateBodyToDirection(-transition.Next.Normal);
                    }
                }
            }
        }

        public override void Exit()
        {
            _movementController.SetGravityDefault();
            if (_inputHandler.MoveForward)
            {
                _playerRotationController.RotateHeadToBody();
            }
            else
            {
                _playerRotationController.RotateBodyToHead();
            }
            _playerRotationController.SyncBodyRotationToHead();
            // TODO фикс метода StickToObstacle
            _characterController.velocity.Set(0, 0, 0);
        }

        private void StickToObstacle(LedgeGrabPoint ledgeGrabPoint)
        {
            var pathToGrabPoint = ledgeGrabPoint.Position - _playerLedgeController.GrabPoint.position;
            // TODO Этот способ задает большие значения velocity у игрока, исправить перемежение в точку захвата
            _characterController.Move(pathToGrabPoint);
        }
    }
}
