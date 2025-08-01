using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Failsafe.Granate.States
{
    [Serializable]
    public class GrenadeStateSettings
    {
        [field: SerializeField] public float DelayExplode { get; private set; }
    }
    public class GrenadeState: GrenadeStateBase
    {
        private readonly GrenadeStateSettings _settings;
        private CancellationTokenSource _explodeDelayCts;
        public GrenadeState(Grenade grenade, GrenadeStateSettings settings) : base(grenade)
        {
            _settings = settings;
        }

        public override void OnStartState()
        {
            //Animations
        }

        public override UseGranateResult OnUsed()
        {
            _explodeDelayCts?.Cancel();
            _explodeDelayCts = new CancellationTokenSource();
            //Throw granate
            DelayExplode(_explodeDelayCts.Token).Forget();
            return new UseGranateResult(true);
        }

        private async UniTaskVoid DelayExplode(CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_settings.DelayExplode), cancellationToken: ct);
            Grenade.Explode();
        }
        
        

        public override void OnStopState()
        {
            //Animations
        }

        public override void Dispose()
        {
            if (_explodeDelayCts != null && !_explodeDelayCts.IsCancellationRequested)
            {
                _explodeDelayCts.Cancel();
                _explodeDelayCts.Dispose();
            }
        }
    }
}