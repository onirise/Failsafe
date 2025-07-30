using System.Linq;
using UnityEngine;

namespace Failsafe.Enemies.Sensors
{
    public class AwarenessMeter
    {
        [Header("Sensors")]
        private readonly Sensor[] _sensors;

        [Header("Настройки")]
        private float _fillSpeed = 80f;
        private float _decaySpeed = 10f;
        private float _decayDelay = 2f;

        [Header("Пороги")] 
        private float _alertThreshold = 30f;
        private float _chaseThreshold = 100f;
        private float _chaseExitThreshold = 30f;
        
        [Range(0, 100)] private float _alertness;

        private readonly Enemy_ScriptableObject _enemyConfig;
        private float _decayDelayTimer;

        private bool _hasEverChased = false;
        private bool _hasLostPlayer = false;

        public float AlertnessValue => _alertness;
        
        public AwarenessMeter(Sensor[] sensors, Enemy_ScriptableObject enemyConfig)
        {
            _sensors = sensors;
            _enemyConfig = enemyConfig;
            
        }

        public void Initialize()
        {
            _fillSpeed = _enemyConfig.FillSpeed;
            _decaySpeed = _enemyConfig.DecaySpeed;
            _decayDelay = _enemyConfig.DecayDelay;

            _alertThreshold = _enemyConfig.AlertThreshold;
            _chaseThreshold = _enemyConfig.ChaseThreshold;
            _chaseExitThreshold = _enemyConfig.ChaseExitThreshold;
        }

        public bool IsChasing()
        {
                if (_alertness >= _chaseThreshold)
                {
                    ApplyAlertSensorParams();
                    _hasEverChased = true;
                    return true;
                }

                return false;
        }

        public bool IsPlayerLost()
        {
            if (_hasEverChased && !_hasLostPlayer && Mathf.Approximately(_alertness, _chaseExitThreshold))
            {
                _hasLostPlayer = true;
                return true;
            }

            return false;
        }

        public bool IsAlerted()
        {
            if (_alertness <= _alertThreshold)
            {
                return false;
            }
            else
            {
                ApplyAlertSensorParams();
                return true;
            }
            
            

        }

        public bool IsCalm()
        {
            // Если враг однажды достиг состояния погони — он не может вернуться в "спокойствие"
            if (_hasEverChased) return false;
            ApplyCalmSensorParams();
            return _alertness <= 0f;
        }

        public void Update()
        {
            if (_alertness >= _chaseThreshold)
            {
                _hasEverChased = true;
                _hasLostPlayer = false; // сбрасываем флаг "игрок потерян"
            }
            float maxSignal = _sensors.Max(s => s.SignalStrength);

            if (maxSignal > 0f)
            {
                _alertness += maxSignal * _fillSpeed * Time.deltaTime;
                _decayDelayTimer = _decayDelay;
            }
            else
            {
                if (_decayDelayTimer > 0f)
                    _decayDelayTimer -= Time.deltaTime;
                else
                    _alertness -= _decaySpeed * Time.deltaTime;
            }

            _alertness = Mathf.Clamp(_alertness, 0f, 100f);

            // Если враг однажды достиг состояния погони — ограничиваем минимальное значение
            if (_hasEverChased)
                _alertness = Mathf.Max(_alertness, _alertThreshold);
        }
        private void SetVisionSensorParams(float distance, float angle, float focusTime)
        {
            foreach (var sensor in _sensors)
            {
                if (sensor is VisualSensor visualSensor)
                {
                    visualSensor.SetDistance(distance);
                    visualSensor.SetAngle(angle);
                    visualSensor.SetFocusingTime(focusTime);
                }
            }
        }

        private void SetHearingSensorParams(float distance, float minSignal, float maxSignal, float focusTime)
        {
            foreach (var sensor in _sensors)
            {
                if (sensor is NoiseSensor noiseSensor)
                {
                    noiseSensor.SetDistance(distance);
                    noiseSensor.SetMinMaxStrength(minSignal, maxSignal);
                    noiseSensor.SetFocusingTime(focusTime);
                }
            }
        }
        
        public void ApplyCalmSensorParams()
        {
            SetVisionSensorParams(_enemyConfig.OriginDistanceSight, _enemyConfig.OrginAngleViev, _enemyConfig.VisualFocusTime);
            SetHearingSensorParams(_enemyConfig.OriginDistanceHearing, _enemyConfig.MinSoundStr, _enemyConfig.MaxSoundStr, _enemyConfig.HearFocusTime);
        }

        public void ApplyAlertSensorParams()
        {
            SetVisionSensorParams(_enemyConfig.AlertDistanceSight, _enemyConfig.AlertAngleViev, _enemyConfig.AlertVisualFocusTime);
            SetHearingSensorParams(_enemyConfig.AlertDistanceHearing, _enemyConfig.AlertMinSoundStr, _enemyConfig.MaxSoundStr, _enemyConfig.AlertHearFocusTime);
        }
    }
    
}
