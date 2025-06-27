using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Failsafe.GameSceneServices.SpawnSystem
{
    /// <summary>
    /// Условие появления противника
    /// </summary>
    public interface ISpawnCondition
    {
        /// <summary>
        /// Проверка выполнения условия
        /// </summary>
        /// <returns>true если условие выполнено</returns>
        public bool IsTriggered();
        /// <summary>
        /// Сбросить параметры условия
        /// </summary>
        public void Reset();
        public IEnumerable<ISpawnCondition> GetChildren();
    }

    public abstract class SpawnCondition : ISpawnCondition
    {
        public abstract bool IsTriggered();

        public virtual void Reset()
        {
        }

        public virtual IEnumerable<ISpawnCondition> GetChildren() => Enumerable.Empty<ISpawnCondition>();
    }

    public class Constant : SpawnCondition
    {
        private bool _value;
        public Constant(bool value)
        {
            _value = value;
        }

        public override bool IsTriggered() => _value;
    }

    public class Timer : SpawnCondition
    {
        private float _startTime = -1;
        private float _delay;
        private ISpawnCondition _startCondition;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="startCondition">Тригер после которого начать таймер, если не указан таймер запускается при создании</param>
        public Timer(float delay, ISpawnCondition startCondition = null)
        {
            _delay = delay;
            if (startCondition != null)
            {
                _startCondition = startCondition;
            }
            else
            {
                Reset();
            }

        }
        public override bool IsTriggered()
        {
            if (_startTime >= 0)
            {
                return _startTime + _delay < Time.time;
            }
            if (_startCondition.IsTriggered())
            {
                Reset();
            }
            return false;
        }

        public override void Reset()
        {
            _startTime = Time.time;
        }
    }

    public class Random : SpawnCondition
    {
        private float _chance;

        public Random(float chance)
        {
            _chance = chance;
            Reset();
        }

        private float _randomNumber;
        public override bool IsTriggered() => _chance >= _randomNumber;

        public override void Reset()
        {
            _randomNumber = UnityEngine.Random.value;
        }
    }

    public class And : ISpawnCondition
    {
        private ISpawnCondition[] _triggers;

        public And(params ISpawnCondition[] triggers)
        {
            _triggers = triggers;
        }

        public bool IsTriggered()
        {
            foreach (var trigger in _triggers)
            {
                if (!trigger.IsTriggered())
                {
                    return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Reset();
            }
        }

        public IEnumerable<ISpawnCondition> GetChildren() => _triggers;
    }

    public class Or : ISpawnCondition
    {
        private ISpawnCondition[] _triggers;

        public Or(params ISpawnCondition[] triggers)
        {
            _triggers = triggers;
        }

        public bool IsTriggered()
        {
            foreach (var trigger in _triggers)
            {
                if (trigger.IsTriggered())
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            foreach (var trigger in _triggers)
            {
                trigger.Reset();
            }
        }

        public IEnumerable<ISpawnCondition> GetChildren() => _triggers;
    }

    public class OtherEnemySpawned : SpawnCondition
    {
        private List<SpawnCandidate> _spawnedEnemies;
        private SpawnCandidate _enemy;

        public OtherEnemySpawned(List<SpawnCandidate> spawnedEnemies, SpawnCandidate enemy)
        {
            _spawnedEnemies = spawnedEnemies;
            _enemy = enemy;
        }

        public override bool IsTriggered()
        {
            foreach (var spawnedEnemy in _spawnedEnemies)
            {
                if (spawnedEnemy == _enemy) return true;
            }
            return false;
        }
    }

    public class SpawnPointPresent : SpawnCondition
    {
        private Dictionary<SpawnPointType, bool> _spawnPoints;
        private SpawnPointType _spawnPointType;

        public SpawnPointPresent(Dictionary<SpawnPointType, bool> spawnPoints, SpawnPointType spawnPointType)
        {
            _spawnPoints = spawnPoints;
            _spawnPointType = spawnPointType;
        }

        public override bool IsTriggered() => _spawnPoints[_spawnPointType];
    }

    public class Trigger : SpawnCondition
    {
        private Func<bool> _trigger;
        private Action _resetTrigger;

        public Trigger(Func<bool> trigger, Action resetTrigger = null)
        {
            _trigger = trigger;
            _resetTrigger = resetTrigger;
        }

        public override bool IsTriggered() => _trigger();

        public override void Reset()
        {
            _resetTrigger?.Invoke();
        }
    }

}