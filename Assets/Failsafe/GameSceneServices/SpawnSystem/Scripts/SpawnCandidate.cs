using System;
using System.Collections.Generic;
using UnityEngine;

namespace Failsafe.GameSceneServices.SpawnSystem
{
    /// <summary>
    /// Данные противника, которые используются системой спауна врагов
    /// </summary>
    public class SpawnCandidate : IEqualityComparer<SpawnCandidate>
    {
        public Guid Id { get; } = Guid.NewGuid();
        /// <summary>
        /// Имя противника
        /// </summary>
        public string Name { get; private set; }
        public GameObject EnemyPrefab { get; private set; }
        /// <summary>
        /// Вес противика, определяет шанс появления
        /// </summary>
        public int Weight { get; private set; }
        /// <summary>
        /// На какой точке может спауниться противник
        /// </summary>
        public SpawnPointType SpawnPointType { get; private set; }

        public SpawnPoint[] SpecificSpawnPoints { get; private set; }

        public SpawnAgent SpawnAgent;

        public SpawnCandidate(string name, GameObject enemyPrefab, int weight, SpawnPointType spawnPointType, SpawnPoint[] specificSpawnPoints = null)
        {
            Name = name;
            EnemyPrefab = enemyPrefab;
            Weight = weight;
            SpawnPointType = spawnPointType;
            SpecificSpawnPoints = specificSpawnPoints?.Length > 0 ? specificSpawnPoints : null;
        }

        public bool Equals(SpawnCandidate x, SpawnCandidate y)
        {
            if (x == null || y == null) return false;
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(SpawnCandidate obj)
        {
            return Id.GetHashCode();
        }
    }
}