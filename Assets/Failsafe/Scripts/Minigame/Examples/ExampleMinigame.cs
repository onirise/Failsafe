using UnityEngine;
using UnityEngine.Events;

public class ExampleMinigame : MinigameBase
{
    [SerializeField] protected UnityEvent _onStart;
    [SerializeField] protected UnityEvent _onExit;

    public void StartGame()
    {
        OnGameStart();
    }

    public void ExitGame()
    {
        OnGameExit();
    }

    public void Win()
    {
        OnWin();
    }

    public void Fail()
    {
        OnFail();
    }

    protected override void OnGameStart()
    {
        print("Game start");
        PerformAction(_onStart);
    }
    protected override void OnGameExit()
    {
        print("Game Exit");
        PerformAction(_onExit);
    }

    protected override void OnWin()
    {
        print("Perform win actions");
        PerformAction(_onWin);
    }

    protected override void OnFail()
    {
        print("Perform fail actions");
        PerformAction(_onFail);
    }
}
