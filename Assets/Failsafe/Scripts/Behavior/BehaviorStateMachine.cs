public class BehaviorStateMachine
{
    private BehaviorState _currentState;

    public BehaviorStateMachine(BehaviorState initState)
    {
        _currentState = initState;
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
}