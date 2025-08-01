using System;
using Cysharp.Threading.Tasks;

public static class Timer
{
    public static async UniTask StartTimer(float totalTime, float tickInterval, Action onTick, Action onComplete)
    {
        float elapsed = 0f;
        while (elapsed < totalTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(tickInterval));
            elapsed += tickInterval;

            onTick?.Invoke();
        }

        onComplete?.Invoke();
    }
}