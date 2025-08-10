using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Состояние
/// </summary>
public abstract class BehaviorState
{
    public event Action OnEnter;
    public event Action OnExit;
    private List<Transition> _transitions = new List<Transition>();
    /// <summary>
    /// Добавить переход к другому состоянию
    /// </summary>
    /// <param name="to">Новое состояние</param>
    /// <param name="condition">Условие перехода</param>
    /// <param name="action">Действие, выполняемое во время смены состояния</param>
    public void AddTransition(BehaviorState to, Func<bool> condition, Action action = null)
    {
        _transitions.Add(new Transition(this, to, condition, action));
    }

    /// <summary>
    /// Решить какой переход выполнить
    /// </summary>
    /// <returns>Первый переход для которого выполнено условие, иначе null</returns>
    public virtual Transition DecideTransition()
    {
        foreach (var transition in _transitions)
        {
            if (transition.Condition()) return transition;
        }
        return null;
    }

    /// <summary>
    /// Вызывается в момент переключения на это состояние
    /// </summary>
    public virtual void Enter()
    {
        OnEnter?.Invoke();
    }

    /// <summary>
    /// Выполняется пока состояние активно
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Выполняется пока состояние активно
    /// </summary>
    public virtual void FixedUpdate() { }

    /// <summary>
    /// Вызывается в момент когда текущее состояние меняется на другое
    /// </summary>
    public virtual void Exit()
    {
        OnExit?.Invoke();
    }

    public IEnumerable<BehaviorState> NextStates() => _transitions.Select(x => x.Next);

    /// <summary>
    /// Переход от одного состояния к другому
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Текущее состояние
        /// </summary>
        public readonly BehaviorState Current;
        /// <summary>
        /// Следующие состояние
        /// </summary>
        public readonly BehaviorState Next;
        /// <summary>
        /// Условие перехода в следующее состояние
        /// </summary>
        public readonly Func<bool> Condition;
        /// <summary>
        /// Действие, выполняемое при переходе в следующее состояние
        /// </summary>
        public readonly Action ActionOnStateChange;

        public Transition(BehaviorState current, BehaviorState next, Func<bool> condition, Action action = null)
        {
            Current = current;
            Next = next;
            Condition = condition;
            ActionOnStateChange = action;
        }
    }
}
