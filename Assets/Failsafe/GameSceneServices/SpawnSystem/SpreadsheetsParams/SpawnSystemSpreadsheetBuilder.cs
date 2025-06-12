using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    public class SpawnSystemSpreadsheetBuilder
    {
        private Dictionary<string, SpawnCandidate> _candidatesByName = new();
        private Dictionary<string, GameObject> _enemyPrefabs = new();

        public SpawnSystemSpreadsheetBuilder(Dictionary<string, GameObject> enemyPrefabs)
        {
            _enemyPrefabs = enemyPrefabs;
        }

        public void BuildSpawnSystem(List<EnemySpawnData> enemySpawnDatas, EnemySpawnSystem spawnSystem)
        {
            foreach (var enemySpawnData in enemySpawnDatas)
            {
                var candidate = new SpawnCandidate(enemySpawnData.Name, _enemyPrefabs[enemySpawnData.Name], enemySpawnData.Weight, Enum.Parse<SpawnPointType>(enemySpawnData.SpawnPointType));
                _candidatesByName[enemySpawnData.Name] = candidate;
            }
            foreach (var enemySpawnData in enemySpawnDatas)
            {
                var candidate = _candidatesByName[enemySpawnData.Name];
                var conditions = ConstructConditions(enemySpawnData, candidate, spawnSystem);
                var agent = new SpawnAgent(new AndCondition(conditions), candidate, enemySpawnData.MaxCount);
                spawnSystem.AddSpawnAgent(agent);
            }
        }

        private ISpawnCondition[] ConstructConditions(EnemySpawnData entity, SpawnCandidate candidate, EnemySpawnSystem spawnSystem)
        {
            var innerConditions = new List<ISpawnCondition>();

            if (entity.Constant)
            {
                innerConditions.Add(new ConstantCondition(true));
            }
            if (entity.Random >= 0)
            {
                innerConditions.Add(new RandomCondition(entity.Random / 100));
            }
            if (entity.Timer >= 0 && entity.Timer <= 100)
            {
                innerConditions.Add(new TimerCondition(entity.Timer / 100));
            }
            if (!string.IsNullOrEmpty(entity.EnemyName) && _candidatesByName.TryGetValue(entity.EnemyName, out var otherCandidate))
            {
                innerConditions.Add(new EnemySpawnedCondition(spawnSystem.SpawnedEnemies, otherCandidate));
            }
            if (entity.UseWeightSystem)
            {
                innerConditions.Add(new TriggerCondition(() => spawnSystem.WeightMeter.CanSpawn(candidate)));

            }
            return innerConditions.ToArray();
        }
    }
}
