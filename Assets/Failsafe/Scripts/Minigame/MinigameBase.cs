using UnityEngine;
using UnityEngine.Events;

public abstract class MinigameBase : MonoBehaviour
{
    [SerializeField] protected UnityEvent _onWin;
    [SerializeField] protected UnityEvent _onFail;

    protected void PerformAction(UnityEvent action)
    {
        action?.Invoke();
    }

    protected virtual void OnGameStart() { }
    protected virtual void OnGameExit() { }
    protected virtual void OnWin() { }
    protected virtual void OnFail() { }
}
