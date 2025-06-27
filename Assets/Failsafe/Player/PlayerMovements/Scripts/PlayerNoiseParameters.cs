using UnityEngine;

namespace Failsafe.PlayerMovements
{
    /// <summary>
    /// Параметры шума, издаваемым игроком
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerNoiseParameters", menuName = "Player/Parameters/PlayerNoiseParameters")]
    public class PlayerNoiseParameters : ScriptableObject
    {
        public float MinStrength = 1f;
        public float DefaultStrength = 5f;
        public float IncreasedStrength = 10f;
        public float ReducedStrength = 2f;
    }
}