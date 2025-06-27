using System.Linq;

namespace Failsafe.GameSceneServices.SpawnSystem
{
    /// <summary>
    /// Агент отвечающий за спаун определенного противника
    /// </summary>
    public class SpawnAgent
    {
        private int _repeat;
        private ISpawnCondition _condition;
        private SpawnCandidate _candidate;

        private bool _isConditionTriggered;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">Услови появление врага</param>
        /// <param name="candidate">Данные противника</param>
        /// <param name="repeat">Сколько раз сработает этот агент</param>
        public SpawnAgent(ISpawnCondition condition, SpawnCandidate candidate, int repeat = 1)
        {
            _repeat = repeat;
            _condition = condition;
            _candidate = candidate;
            _candidate.SpawnAgent = this;
        }

        /// <summary>
        /// Выполнено ли условие спауна врага
        /// </summary>
        /// <returns></returns>
        public bool IsConditionTringered()
        {
            if (_repeat <= 0) return false;
            if (_isConditionTriggered) return true;
            _isConditionTriggered = _condition.IsTriggered();
            return _isConditionTriggered;
        }

        /// <summary>
        /// Враг которого нужно заспаунить
        /// </summary>
        /// <returns></returns>
        public SpawnCandidate GetSpawnCandidate()
        {
            return _candidate;
        }

        public void Reset()
        {
            _isConditionTriggered = false;
            _condition.Reset();
        }

        public void Spawned()
        {
            _repeat--;
        }

        public override string ToString()
        {
            var result = $"SpawnAgent for {_candidate.Name} with conditions: {_condition.GetType().Name} ";
            result = ToStringChilds(result, _condition);
            return result;
        }

        private string ToStringChilds(string result, ISpawnCondition condition)
        {
            if (!condition.GetChildren().Any()) return result;
            result += "( ";
            foreach (var child in condition.GetChildren())
            {
                result += child.GetType().Name + ", ";
                ToStringChilds(result, child);
            }

            return result += ") ";
        }
    }
}