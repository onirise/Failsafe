using System.Collections.Generic;
using System.Linq;

public class BehaviorStateMachine
{
    private BehaviorState _currentState;
    public BehaviorState CurrentState => _currentState;
    private List<BehaviorState> _states = new List<BehaviorState>();
    private List<BehaviorForcedState> _forcedStates;

    /// <summary>
    /// Конструктор машины состояний.
    /// </summary>
    /// <param name="initState">Начальное состояние</param>
    /// <param name="forcedStates">Список принудительных состояний</param>
    public BehaviorStateMachine(BehaviorState initState, List<BehaviorForcedState> forcedStates = null)
    {
        _currentState = initState;
        _forcedStates = forcedStates ?? new List<BehaviorForcedState>();
        _currentState.Enter();
        CacheAllStates();
    }

    /// <summary>
    /// Конструктор машины состояний.
    /// </summary>
    /// <remarks>
    /// После создания нужно обязательно вызвать метод <see cref="SetInitState"/>
    /// </remarks>
    /// <param name="forcedStates">Список принудительных состояний</param>
    public BehaviorStateMachine(List<BehaviorForcedState> forcedStates = null)
    {
        _forcedStates = forcedStates ?? new List<BehaviorForcedState>();
    }

    /// <summary>
    /// Задать начальное состояние машины
    /// </summary>
    /// <param name="initState"></param>
    public void SetInitState(BehaviorState initState)
    {
        _currentState = initState;
        _currentState.Enter();
        CacheAllStates();
    }

    /// <summary>
    /// Получить Состояние машины
    /// </summary>
    /// <typeparam name="T">Тип состояния</typeparam>
    /// <returns>null если состояния типа <see cref="T"/> не найдено</returns>
    public T GetState<T>() where T : BehaviorState
    {
        return _states.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
    }

    /// <summary>
    /// Вызывать в методе MonoBehaviour.Update
    /// </summary>
    public void Update()
    {
        _currentState.Update();
        var transition = _currentState.DecideTransition();
        if (transition == null) return;

        var nextState = transition.Next;
        if (nextState != _currentState)
        {
            _currentState.Exit();
            _currentState = nextState;
            _currentState.Enter();
            transition.ActionOnStateChange?.Invoke();
        }
    }

    /// <summary>
    /// Вызывать в методе MonoBehaviour.FixedUpdate
    /// </summary>
    public void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    /// <summary>
    /// Переключение на принудительное состояние
    /// </summary>
    public void ForseChangeState<T>(float? duration = null) where T : BehaviorForcedState
    {
        var nextState = _forcedStates.FirstOrDefault(x => x.GetType() == typeof(T));
        if (nextState != null)
        {
            ForseChangeState(nextState, duration);
        }
    }

    private void ForseChangeState(BehaviorForcedState nextState, float? duration)
    {
        if (nextState != _currentState)
        {
            var prevState = _currentState;
            prevState.Exit();
            _currentState = nextState;
            nextState.Enter(prevState, duration);
        }
    }

    private void CacheAllStates()
    {
        var currentState = _currentState;
        _states.Add(currentState);
        AddChildStates(currentState);
    }

    private void AddChildStates(BehaviorState state)
    {
        foreach (var nextState in state.NextStates())
        {
            if (_states.Contains(nextState)) continue;
            _states.Add(nextState);
            AddChildStates(nextState);
        }
    }
}