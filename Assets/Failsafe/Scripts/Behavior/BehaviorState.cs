using System;
using System.Collections.Generic;
/// <summary>
/// Состояние
/// </summary>
public abstract class BehaviorState
{
    private List<Transition> _transitions = new List<Transition>();
    /// <summary>
    /// Добавить переход к другому состоянию
    /// </summary>
    /// <param name="to">Новое состояние</param>
    /// <param name="condition">Условие перехода</param>
    public void AddTransition(BehaviorState to, Func<bool> condition)
    {
        _transitions.Add(new Transition(this, to, condition));
    }

    /// <summary>
    /// Решить на основе переходов какое состояние следующее
    /// </summary>
    /// <returns>Первое состояние для которого выполнено условие перехода, иначе текущее состояние</returns>
    public virtual BehaviorState DecideNextState()
    {
        foreach (var transition in _transitions)
        {
            if (transition.condition())
                return transition.next;
        }
        return this;
    }

    /// <summary>
    /// Вызывается в момент переключения на это состояние
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// Выполняется пока состояние активно
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Вызывается в момент когда текущее состояние меняется на другое
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// Переход от одного состояния к другому
    /// </summary>
    private class Transition
    {
        public readonly BehaviorState current;
        public readonly BehaviorState next;
        public readonly Func<bool> condition;

        public Transition(BehaviorState current, BehaviorState next, Func<bool> condition)
        {
            this.current = current;
            this.next = next;
            this.condition = condition;
        }
    }
}
