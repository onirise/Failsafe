using System;

namespace SpawnSystem
{
    [Serializable]
    public class EnemySpawnData
    {
        public string Name;
        public int Weight;
        public int MaxCount;
        public string SpawnPointType;
        public bool Constant;
        public float Random;
        public float Timer;
        public int Level;
        public string EnemyName;
        public bool UseWeightSystem;
    }
}
