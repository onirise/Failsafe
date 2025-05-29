using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Уровень шума игрока
    /// </summary>
    public enum PlayerNoiseVolume { Minimum, Default, Reduced, Increased }
    /// <summary>
    /// Управление шумом, издаваемым игроком
    /// </summary>
    public class PlayerNoiseController
    {
        private readonly PlayerNoiseParameters _playerNoiseParametrs;
        private readonly Transform _playerTransform;
        private readonly SignalChannel _playerSignalChannel;
        private readonly PlayerNoiseSignal _noiseSignal;

        public PlayerNoiseController(Transform playerTransform, PlayerNoiseParameters playerNoiseParametrs)
        {
            _playerTransform = playerTransform;
            _playerNoiseParametrs = playerNoiseParametrs;
            _playerSignalChannel = SignalManager.Instance.PlayerNoiseChanel;
            _noiseSignal = new PlayerNoiseSignal(_playerTransform);
            _playerSignalChannel.AddConstant(_noiseSignal);
        }

        /// <summary>
        /// Установить шум, издаваемый игроком
        /// </summary>
        /// <param name="volume"></param>
        public void SetNoiseStrength(PlayerNoiseVolume volume)
        {
            float strength = volume switch
            {
                PlayerNoiseVolume.Default => _playerNoiseParametrs.DefaultStrength,
                PlayerNoiseVolume.Reduced => _playerNoiseParametrs.ReducedStrength,
                PlayerNoiseVolume.Increased => _playerNoiseParametrs.IncreasedStrength,
                _ => _playerNoiseParametrs.MinStrength
            };
            SetNoiseStrength(strength);
        }

        /// <summary>
        /// Установить шум, издаваемый игроком
        /// </summary>
        /// <param name="strength"></param>
        private void SetNoiseStrength(float strength)
        {
            _noiseSignal.UpdateStrength(strength);
        }

        /// <summary>
        /// Создать шум в точке персонажа
        /// </summary>
        /// <param name="strength">Сила шума</param>
        /// <param name="duration">Длительность</param>
        public void CreateNoise(float strength, float duration = 5f)
        {
            _playerSignalChannel.Add(_playerTransform.position, strength, duration);
        }
    }
}