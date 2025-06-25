using UnityEngine;

namespace Failsafe.GameSceneServices.SpawnSystem
{
    [CreateAssetMenu(fileName = "EnemySpawnPrefabSO", menuName = "SpawnSystem/EnemySpawnPrefabSO")]
    public class EnemySpawnPrefabSO : ScriptableObject
    {
        public string Name;
        public GameObject EnemyPrefab;
    }
}
