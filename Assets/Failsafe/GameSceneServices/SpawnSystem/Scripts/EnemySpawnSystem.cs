using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Failsafe.GameSceneServices.SpawnSystem
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
    public class EnemySpawnSystem : IStartable, ITickable, IEnemySpawnSystem
    {
        private SpawnPoint[] _spawnPoints;
        private Dictionary<SpawnPointType, bool> _spawnPointTypePresent = new Dictionary<SpawnPointType, bool>();
        private List<SpawnCandidate> _spawnedEnemies = new List<SpawnCandidate>();
        private List<SpawnAgent> _spawnAgents = new List<SpawnAgent>();

        private List<SpawnCandidate> _spawnCandidates = new List<SpawnCandidate>();

        private bool OnDelay => _lastSpawnCheckAt + _spawnCheckDelay > Time.time;
        private float _spawnCheckDelay = 1;
        private float _lastSpawnCheckAt;

        private bool IsActive => _activateAt < Time.time;
        private float _activateAt;

        private WeightMeter _weightMeter = new WeightMeter();
        public WeightMeter WeightMeter => _weightMeter;

        private EnemySpawnSystemBuilder _enemySpawnSystemBuilder;
        private readonly IObjectResolver _objectResolver;

        public List<SpawnCandidate> SpawnedEnemies => _spawnedEnemies;


        public EnemySpawnSystem(EnemySpawnSystemBuilder enemySpawnSystemBuilder, IObjectResolver objectResolver)
        {
            _enemySpawnSystemBuilder = enemySpawnSystemBuilder;
            _objectResolver = objectResolver;
        }

        public void Deactivate(float duration)
        {
            _activateAt = duration;
        }

        public void AddSpawnAgent(SpawnAgent spawnAgent)
        {
            _spawnAgents.Add(spawnAgent);
        }

        public void Start()
        {
            Debug.Log("EnemySpawnSystem Start");
            _spawnPoints = LifetimeScope.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
            foreach (SpawnPointType spawnPointType in Enum.GetValues(typeof(SpawnPointType)))
            {
                _spawnPointTypePresent.Add(spawnPointType, false);
            }
            foreach (var spawnPoint in _spawnPoints)
            {
                _spawnPointTypePresent[spawnPoint.Type] = true;
            }
            if (_enemySpawnSystemBuilder != null)
                _enemySpawnSystemBuilder.BuildSpawnSystem(this);
        }

        private void TestBuild()
        {
            var candidate1 = new SpawnCandidate("Enemy1", null, 5, SpawnPointType.Default);
            var condition1 = new Or(
                new And(
                    new Random(0.5f),
                    new Timer(2)),
                new Timer(5));
            var agent1 = new SpawnAgent(condition1, candidate1, 2);

            var candidate2 = new SpawnCandidate("Enemy2", null, 10, SpawnPointType.Default);
            var condition2 = new OtherEnemySpawned(_spawnedEnemies, candidate1);
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

        public void Tick()
        {
            if (!IsActive) return;
            if (OnDelay) return;
            if (_spawnPoints.Length == 0) return;

            _lastSpawnCheckAt = Time.time;
            if (_spawnCandidates.Count == 0)
            {
                if (!HasSpawnCandidates()) return;
            }
            var (candidate, spawnPoint) = ChooseCandidateAndSpawnPoint();

            SpawnEnemy(candidate, spawnPoint);

            foreach (var agent in _spawnAgents)
            {
                if (agent.IsConditionTringered())
                    agent.Reset();
            }
        }

        private void SpawnEnemy(SpawnCandidate candidate, SpawnPoint spawnPoint)
        {
            Debug.Log($"[{nameof(EnemySpawnSystem)}] Try spawn enemy {candidate?.Name} at position {spawnPoint?.Position}");
            if (spawnPoint == null)
                return;
            var enemy = _objectResolver.Instantiate(candidate.EnemyPrefab, spawnPoint.Position, spawnPoint.Rotation);

            _spawnedEnemies.Add(candidate);
            _weightMeter.AddWeight(candidate.Weight);
            candidate.SpawnAgent.Spawned();
            spawnPoint.EnemySpawned();
            _spawnCandidates.Clear();
        }

        private (SpawnCandidate, SpawnPoint) ChooseCandidateAndSpawnPoint()
        {
            var spawnCandidate = GetRandom(_spawnCandidates);
            var spawnPoints = spawnCandidate.SpecificSpawnPoints.AsEnumerable()
                ?? _spawnPoints.Where(x => x.Type == spawnCandidate.SpawnPointType);
            var spawnPoint = GetRandom(spawnPoints.Where(x => x.Enabled));

            return (spawnCandidate, spawnPoint);
        }

        private static T GetRandom<T>(IEnumerable<T> list)
        {
            var countLength = list.Count();
            if (countLength <= 1) return list.FirstOrDefault();
            var i = UnityEngine.Random.Range(0, countLength);
            return list.ElementAt(i);
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