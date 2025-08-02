using System;
using Cysharp.Threading.Tasks;

public static class Timer
{
    public static async UniTask StartTimer(float totalTime, float tickInterval = 1, Action onTick = null, Action onComplete = null, bool allowFinalAction = false)
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