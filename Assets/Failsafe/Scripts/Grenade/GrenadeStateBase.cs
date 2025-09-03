using System;
using UnityEngine;

namespace Failsafe.Grenade.States
{
    [Serializable]
    public class UseGrenadeResult
    {
        public readonly bool Success;

        public UseGrenadeResult(bool success)
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
        public abstract UseGrenadeResult OnUsed(Vector3 direction);
        public abstract void OnStopState();
        public abstract void Dispose();
    }
}