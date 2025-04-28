using UnityEngine;

namespace Failsafe.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField]
        public string MainMenuSceneName { get; private set; }
    }
}