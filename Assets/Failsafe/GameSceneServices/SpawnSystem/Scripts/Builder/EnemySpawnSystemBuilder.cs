using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Failsafe.GameSceneServices.SpawnSystem
{
    public class EnemySpawnSystemBuilder : MonoBehaviour
    {
        [SerializeField] private EnemySpawnData[] _enemySpawnDatas;

        private Dictionary<string, SpawnCandidate> _candidatesByName = new();
        private Dictionary<string, GameObject> _enemyPrefabs = new();

        public void BuildSpawnSystem(EnemySpawnSystem spawnSystem)
        {
            Debug.Log("BuildSpawnSystem");
            _enemyPrefabs = _enemySpawnDatas.Select(x => x.EnemySpawnPrefab).ToDictionary(x => x.Name, y => y.EnemyPrefab, StringComparer.OrdinalIgnoreCase);
            foreach (var enemySpawnData in _enemySpawnDatas)
            {
                if (_candidatesByName.ContainsKey(enemySpawnData.EnemySpawnPrefab.Name))
                    continue;
                var candidate = new SpawnCandidate(
                    enemySpawnData.EnemySpawnPrefab.Name,
                    _enemyPrefabs[enemySpawnData.EnemySpawnPrefab.Name],
                    enemySpawnData.Weight,
                    enemySpawnData.SpawnPointType,
                    enemySpawnData.SpecificSpawnPoints);
                _candidatesByName.Add(enemySpawnData.EnemySpawnPrefab.Name, candidate);
            }
            foreach (var enemySpawnData in _enemySpawnDatas)
            {
                var candidate = _candidatesByName[enemySpawnData.EnemySpawnPrefab.Name];
                var conditions = ConstructConditions(enemySpawnData, candidate, spawnSystem);
                var agent = new SpawnAgent(new And(conditions), candidate, enemySpawnData.MaxCount);
                spawnSystem.AddSpawnAgent(agent);
            }
        }

        private ISpawnCondition[] ConstructConditions(EnemySpawnData entity, SpawnCandidate candidate, EnemySpawnSystem spawnSystem)
        {
            var innerConditions = new List<ISpawnCondition>();

            if (entity.Random >= 0)
            {
                innerConditions.Add(new Random(entity.Random / 100));
            }
            if (entity.Timer >= 0 && entity.Timer <= 100)
            {
                innerConditions.Add(new Timer(entity.Timer / 100));
            }
            if (!string.IsNullOrEmpty(entity.OtherEnemyName) && _candidatesByName.TryGetValue(entity.OtherEnemyName, out var otherCandidate))
            {
                innerConditions.Add(new OtherEnemySpawned(spawnSystem.SpawnedEnemies, otherCandidate));
            }
            if (entity.UseWeightSystem)
            {
                innerConditions.Add(new Trigger(() => spawnSystem.WeightMeter.CanSpawn(candidate)));

            }
            return innerConditions.ToArray();
        }
    }
}