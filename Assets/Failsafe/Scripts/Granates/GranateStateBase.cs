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
    public abstract class GranateStateBase: IDisposable
    {
        protected readonly Granate _granate;

        protected GranateStateBase(Granate granate)
        {
            _granate = granate;
        }
        public abstract void OnStartState();
        public abstract UseGranateResult OnUsed();
        public abstract void OnStopState();
        public abstract void Dispose();
    }
}