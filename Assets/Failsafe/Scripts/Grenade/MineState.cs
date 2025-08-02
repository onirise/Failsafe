using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Failsafe.Grenade.States
{
    [Serializable]
    public class MineStateSettings
    {
        [field: SerializeField] public MineObject MinePrefab { get; private set; }
        [field: SerializeField] public float RayCastDist { get; private set; }
        [field: SerializeField] public LayerMask LayerMaskPlentable { get; private set; }
        [field: SerializeField] public Vector3 MineOffset { get; private set; }
    }
    public class MineState: GrenadeStateBase
    {
        private readonly MineStateSettings _settings;
        
        public MineState(Grenade grenade, MineStateSettings settings) : base(grenade)
        {
            _settings = settings;
        }

        public override void OnStartState()
        {
            //Animations
        }

        public override UseGrenadeResult OnUsed(Vector3 direction)
        {
            return new UseGrenadeResult(true);
        }

        public override void OnStopState()
        {
            //Animations
        }

        public override void Dispose(){}
    }
}