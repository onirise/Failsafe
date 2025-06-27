using System;

namespace Failsafe.GameSceneServices.SpawnSystem
{
    [Serializable]
    public struct EnemySpawnData
    {
        public EnemySpawnPrefabSO EnemySpawnPrefab;
        public int Weight;
        public int MaxCount;
        public SpawnPointType SpawnPointType;
        public SpawnPoint[] SpecificSpawnPoints;
        public float Random;
        public float Timer;
        public int Level;
        public string OtherEnemyName;
        public bool UseWeightSystem;
    }
}
