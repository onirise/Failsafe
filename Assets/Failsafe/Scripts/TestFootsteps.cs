using UnityEngine;
using FMODUnity;

namespace Failsafe.PlayerMovements.Controllers
{
    public class StepController
    {
        private readonly CharacterController _cc;
        private readonly PlayerNoiseController _noise;
        private readonly EventReference _footstepEvent;

        private float _stepTimer;

        private readonly float _fixedStepInterval = 0.4f;
        private readonly float _minSpeedToStep = 0.2f;

        public StepController(CharacterController cc, PlayerNoiseController noise, EventReference footstepEvent)
        {
            _cc = cc;
            _noise = noise;
            _footstepEvent = footstepEvent;
        }

        public void Update(bool isActive)
        {
            if (!isActive || _footstepEvent.IsNull || !_cc.isGrounded)
                return;

            Vector3 flatVel = new Vector3(_cc.velocity.x, 0, _cc.velocity.z);
            float speed = flatVel.magnitude;

            // Не двигаемся — ничего не делаем, но таймер не сбрасываем
            if (speed < _minSpeedToStep)
                return;

            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0f)
            {
                RuntimeManager.PlayOneShot(_footstepEvent, _cc.transform.position);
                _noise.CreateNoise(1f, 0.2f);
                _stepTimer = _fixedStepInterval;
            }
        }
    }
}