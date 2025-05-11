using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    /// <summary>
    /// Система спауна врагов
    /// </summary>
    public interface IEnemySpawnSystem
    {
        /// <summary>
        /// Отключение спауна врагов на определенное время
        /// </summary>
        /// <param name="duration"></param>
        public void Deactivate(float duration);
    }

    /// <summary>
    /// Система спауна врагов
    /// </summary>
    public class EnemySpawnSystem : MonoBehaviour, IEnemySpawnSystem
    {
        [SerializeField]
        private SpawnPoint[] _spawnPoints;
        private Dictionary<SpawnPointType, bool> _spawnPointTypePresent = new Dictionary<SpawnPointType, bool>();
        private List<SpawnCandidate> _spawnedEnemies = new List<SpawnCandidate>();
        private List<SpawnAgent> _spawnAgents = new List<SpawnAgent>();

        private List<SpawnCandidate> _spawnCandidates = new List<SpawnCandidate>();

        private bool OnDelay => _lastSpawnCheckAt + _spawnCheckDelay > Time.time;
        [SerializeField]
        private float _spawnCheckDelay = 1;
        [SerializeField]
        private float _lastSpawnCheckAt;

        private bool IsActive => _activateAt < Time.time;
        private float _activateAt;

        private WeightMeter _weightMeter = new WeightMeter();
        public WeightMeter WeightMeter => _weightMeter;

        [SerializeField]
        private SpawnSystemSpreadsheetContainer _spawnSystemSpreadsheet;
        [SerializeField]
        private EnemyPrefabsSO _enemyPrefabsSO;

        public List<SpawnCandidate> SpawnedEnemies => _spawnedEnemies;

        public void Deactivate(float duration)
        {
            _activateAt = duration;
        }

        public void AddSpawnAgent(SpawnAgent spawnAgent)
        {
            _spawnAgents.Add(spawnAgent);
        }

        void Start()
        {
            _spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            foreach (SpawnPointType spawnPointType in Enum.GetValues(typeof(SpawnPointType)))
            {
                _spawnPointTypePresent.Add(spawnPointType, false);
            }
            foreach (var spawnPoint in _spawnPoints)
            {
                _spawnPointTypePresent[spawnPoint.type] = true;
            }
            var builder = new SpawnSystemSpreadsheetBuilder(_enemyPrefabsSO.ToDictionary());
            builder.BuildSpawnSystem(_spawnSystemSpreadsheet.Content.enemySpawnDatas, this);
        }

        private void TestBuild()
        {
            var candidate1 = new SpawnCandidate("Enemy1", null, 5, SpawnPointType.Default);
            var condition1 = new OrCondition(
                new AndCondition(
                    new RandomCondition(0.5f),
                    new TimerCondition(2)),
                new TimerCondition(5));
            var agent1 = new SpawnAgent(condition1, candidate1, 2);

            var candidate2 = new SpawnCandidate("Enemy2", null, 10, SpawnPointType.Default);
            var condition2 = new EnemySpawnedCondition(_spawnedEnemies, candidate1);
            var agent2 = new SpawnAgent(condition2, candidate2, 5);

            _spawnAgents.Add(agent1);
            _spawnAgents.Add(agent2);
        }

        private bool HasSpawnCandidates()
        {
            bool hasCandidate = false;
            foreach (var agent in _spawnAgents)
            {
                if (agent.IsConditionTringered())
                {
                    _spawnCandidates.Add(agent.GetSpawnCandidate());
                    hasCandidate = true;
                }
            }
            return hasCandidate;
        }

        void Update()
        {
            if (!IsActive) return;
            if (OnDelay) return;

            _lastSpawnCheckAt = Time.time;
            if (_spawnCandidates.Count == 0)
            {
                if (!HasSpawnCandidates()) return;
            }
            var (candidate, spawnPoint) = ChooseCandidateAndSpawnPoint();

            //Instantiate(candidate.EnemyPrefab, spawnPoint.Position, spawnPoint.Rotation);
            Debug.Log($"[{nameof(EnemySpawnSystem)}] Spawned enemy {candidate.Name} at position {spawnPoint.Position}");

            _spawnedEnemies.Add(candidate);
            _weightMeter.AddWeight(candidate.Weight);
            candidate.spawnAgent.Spawned();
            _spawnCandidates.Clear();

            foreach (var agent in _spawnAgents)
            {
                if (agent.IsConditionTringered())
                    agent.Reset();
            }
        }

        private (SpawnCandidate, SpawnPoint) ChooseCandidateAndSpawnPoint()
        {
            // TODO: переписать выбор врага и точки спауна
            var spawnCandidate = GetRandom(_spawnCandidates);
            var spawnPoint = GetRandom(_spawnPoints);

            return (spawnCandidate, spawnPoint);
        }

        private static T GetRandom<T>(IReadOnlyList<T> list)
        {
            if (list.Count == 0) return default;
            var i = UnityEngine.Random.Range(0, list.Count);
            return list[i];
        }
    }

    /// <summary>
    /// Шкала веса противников на уровне
    /// </summary>
    public class WeightMeter
    {
        public int MaxWeight => 1000;
        public int CurrentWeight { get; private set; }

        public bool CanSpawn(SpawnCandidate candidate) => CurrentWeight + candidate.Weight <= MaxWeight;

        public void AddWeight(int weight)
        {
            CurrentWeight += weight;
        }
    }
}