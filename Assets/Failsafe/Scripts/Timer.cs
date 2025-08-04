using System;
using Cysharp.Threading.Tasks;

public class Timer
{
    public float TickInterval
    {
        get => _tickInterval;
        
        set
        {
            if (value > 0f)
                _tickInterval = value;
        }
    }
    
    private float _tickInterval;
    
    private bool _isRunning;
    private Action _onTick;

    public Timer(Action onTick, float tickInterval)
    {
        _onTick = onTick;
        _tickInterval = tickInterval;
    }
    
    public void StopTimer()
    {
        _isRunning = false;
    }

    public async UniTask StartTimer()
    {
        _isRunning = true;
        while (_isRunning)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_tickInterval));
            if (_isRunning)
                _onTick?.Invoke();
        }
    }
    
    
    public static async UniTask TimerAsync(float totalTime, float tickInterval = 1, Action onTick = null,
        Action onComplete = null, bool allowFinalAction = false)
    {
        float elapsed = 0f;
        while (elapsed < totalTime)
        {
            if (elapsed + tickInterval > totalTime)
            {
                float remain = totalTime - elapsed;
                await UniTask.Delay(TimeSpan.FromSeconds(remain));

                if (allowFinalAction)
                    onTick?.Invoke();

                break;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(tickInterval));
            elapsed += tickInterval;

            onTick?.Invoke();
        }

        onComplete?.Invoke();
    }
}