
/// <summary>
/// Принудительное состояние
/// Состояние, которое задается снаружи машины состояний
/// </summary>
public abstract class BehaviorForcedState : BehaviorState
{
    /// <summary>
    /// Предыдущее состояние на момент переключения на это состояние
    /// </summary>
    protected BehaviorState _previousState;

    /// <summary>
    /// Вызывается в момент переключения на это состояние
    /// </summary>
    /// <param name="previousState">Предыдущее состояние на момент переключения на это состояние</param>
    public virtual void Enter(BehaviorState previousState)
    {
        Enter();
        _previousState = previousState;
    }

    public override abstract BehaviorState DecideNextState();
}
