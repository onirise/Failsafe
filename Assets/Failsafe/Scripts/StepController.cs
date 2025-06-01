using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    public class StepController
    {
        private readonly CharacterController _characterController;
        private readonly EventReference _footstepEvent;

        private float _stepTimer = 0.6f;
        private bool _enabled = false;

        // Интервалы между шагами
        private  float _minStepInterval = 0.4f;
        private  float _maxStepInterval = 0.6f;

   

        public StepController(
            CharacterController characterController,
            PlayerMovementParameters movementParams,
            EventReference footstepEvent)
        {
            _characterController = characterController;
            _footstepEvent = footstepEvent;
        }

        public void Enable(float speed)
        {
            _enabled = true;
            _minStepInterval = Mathf.Clamp(2f / speed - 0.1f, 0.1f, 1f);
            _maxStepInterval = Mathf.Clamp(2f / speed + 0.1f, 0.1f, 2f);
            _stepTimer = 0.1f;

        }
        public void Disable() => _enabled = false;
        public void ResetTimer() => _stepTimer = 0f;

        public void Update()
        {
            if (!_enabled)
            {
                return;
            }
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0f)
            {
                PlayFootstep();
                _stepTimer = UnityEngine.Random.Range(_minStepInterval, _maxStepInterval);
            }
        }

        private void PlayFootstep()
        {
            RuntimeManager.PlayOneShot(_footstepEvent, _characterController.transform.position);
        }
    }
}