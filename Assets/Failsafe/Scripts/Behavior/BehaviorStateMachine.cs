using System.Collections.Generic;
using System.Linq;

public class BehaviorStateMachine
{
    private BehaviorState _currentState;
    private List<BehaviorForcedState> _forcedStates;

    public BehaviorStateMachine(BehaviorState initState, List<BehaviorForcedState> forcedStates = null)
    {
        _currentState = initState;
        _forcedStates = forcedStates ?? new List<BehaviorForcedState>();
    }

    /// <summary>
    /// Вызывать в методе MonoBehaviour.Update
    /// </summary>
    public void Update()
    {
        var nextState = _currentState.DecideNextState();
        if (nextState != _currentState)
        {
            _currentState.Exit();
            _currentState = nextState;
            _currentState.Enter();
        }
        _currentState.Update();
    }

    /// <summary>
    /// Переключение на принудительное состояние
    /// </summary>
    public void ForseChangeState<T>() where T : BehaviorForcedState
    {
        var nextState = _forcedStates.FirstOrDefault(x => x.GetType() == typeof(T));
        if (nextState != null)
        {
            ForseChangeState(nextState);
        }
    }

    private void ForseChangeState(BehaviorForcedState nextState)
    {
        if (nextState != _currentState)
        {
            var prevState = _currentState;
            prevState.Exit();
            _currentState = nextState;
            nextState.Enter(prevState);
        }
    }
}