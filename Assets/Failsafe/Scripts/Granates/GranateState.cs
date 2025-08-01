using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Failsafe.Granate.States
{
    [Serializable]
    public class GranateStateSettings
    {
        [field: SerializeField] public float DelayExplode { get; private set; }
    }
    public class GranateState: GranateStateBase
    {
        private readonly GranateStateSettings _settings;
        private CancellationTokenSource _explodeDelayCts;
        public GranateState(Granate granate, GranateStateSettings settings) : base(granate)
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
            _granate.Explode();
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