using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    /// <summary>
    /// Данные противника, которые используются системой спауна врагов
    /// </summary>
    public class SpawnCandidate : IEqualityComparer<SpawnCandidate>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string name;
        public GameObject enemyPrefab;
        /// <summary>
        /// Вес противика, определяет шанс появления
        /// </summary>
        public int weight;
        /// <summary>
        /// На какой точке может спауниться противник
        /// </summary>
        public SpawnPointType spawnPointType;

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