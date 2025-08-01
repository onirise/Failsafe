using System;

namespace Failsafe.Granate.States
{
    [Serializable]
    public class UseGranateResult
    {
        public readonly bool Success;

        public UseGranateResult(bool success)
        {
            Success = success;
        }
    }
    public abstract class GrenadeStateBase: IDisposable
    {
        protected readonly Grenade Grenade;

        protected GrenadeStateBase(Grenade grenade)
        {
            Grenade = grenade;
        }
        public abstract void OnStartState();
        public abstract UseGranateResult OnUsed();
        public abstract void OnStopState();
        public abstract void Dispose();
    }
}