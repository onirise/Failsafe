using Failsafe.Player.View;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Failsafe.Player
{
    /// <summary>
    /// Привязывает позицию камеры к голове персонажа и к повороту рига
    /// </summary>
    public class PlayerCameraController : ILateTickable
    {
        private readonly Transform _playerCamera;
        private readonly Transform _playerRigHead;
        private readonly Transform _playerModelHead;
        private Vector3 _initialCameraOffset;

        public PlayerCameraController(PlayerView playerView)
        {
            _playerCamera = playerView.PlayerCamera;
            _playerRigHead = playerView.PlayerRigHead;
            _playerModelHead = playerView.PlayerModelHead;
            _initialCameraOffset = _playerCamera.position - _playerModelHead.position;
        }

        public void LateTick()
        {
            _playerCamera.localRotation = _playerRigHead.localRotation;
            _playerCamera.position = _playerModelHead.position + _playerRigHead.rotation * _initialCameraOffset;
        }
    }
}

