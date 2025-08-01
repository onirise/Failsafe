using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Failsafe.Granate.States
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

        private readonly RaycastHit[] _hits = new RaycastHit[1];
        private readonly Camera _camera;
        public MineState(Grenade grenade, MineStateSettings settings) : base(grenade)
        {
            _settings = settings;
            
            //взять ссылку на камеру нормально
            _camera = Camera.main;
        }

        public override void OnStartState()
        {
            //Animations
        }

        public override UseGranateResult OnUsed()
        {
            int count = Physics.RaycastNonAlloc(_camera.transform.position, _camera.transform.forward, _hits, 
                _settings.RayCastDist, _settings.LayerMaskPlentable);
            if (count > 0)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, _hits[0].normal);
                
                Object.Instantiate(_settings.MinePrefab, _hits[0].point, targetRotation);
                return new UseGranateResult(true);
            }
            return new UseGranateResult(false);
        }

        public override void OnStopState()
        {
            //Animations
        }

        public override void Dispose(){}
    }
}