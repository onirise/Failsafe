using Cysharp.Threading.Tasks;
using Failsafe.Granate.States;
using Failsafe.Scripts.Damage.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Failsafe.Granate
{
    [Serializable]
    public enum GranateStateName
    {
        MINE,
        GRANATE
    }
    public class Granate : MonoBehaviour, IDisposable
    {
        [SerializeField] private GranateData _data;
        [SerializeField] private GranateStateName _startState;
        
        [SerializeField] private KeyCode _switchGranateKey = KeyCode.H;

        [Space(15)]
        [Header("StatesSettings")]
        
        [SerializeField] private MineStateSettings _mineStateSettings;

        [SerializeField] private GranateStateSettings _granateStateSettings;
        
        private GranateStateBase _currentState;
        private GranateStateName _currentStateName;

        public GranateStateName GetCurrentStateName => _currentStateName;
        
        private List<GranateStateBase> _allGranateStates;
        
        private CancellationTokenSource _explodeCts;
        private RaycastHit[] _hits;
        private readonly HashSet<Transform> _hitsTransforms = new();
        
        public event Action OnExplode;


        private bool _isUsed;

        private void Awake()
        {
            _hits = new RaycastHit[_data.HitsCount];
            
            Initialize();
        }

        private void Start()
        {
            SwitchState(_startState);
        }

        private void Initialize()
        {
            _allGranateStates = new List<GranateStateBase>()
            {
                new GranateState(this, _granateStateSettings),
                new MineState(this, _mineStateSettings)
            };
        }
        
        //вызывается извне
        public void Use()
        {
            _isUsed = true;
            if (_currentState.OnUsed().Success)
            {
                //Success sound
            }
            else
            {
                //No success sound
            }
        }
        
        private void Update()
        {
            if(_isUsed)return;
            if (Input.GetKeyDown(_switchGranateKey))
            {
                SwitchState(IncrementEnumCyclic(_currentStateName));
            }
        }
        
        private GranateStateName IncrementEnumCyclic(GranateStateName current)
        {
            GranateStateName[] values = (GranateStateName[])Enum.GetValues(typeof(GranateStateName));
            
            int currentInt = (int)current;
            int nextInt = (currentInt + 1) % values.Length;
            return (GranateStateName)nextInt;
        }


        public void Explode()
        {
            //Эффекты через вьюшку
            OnExplode?.Invoke();
            
            _explodeCts?.Cancel();
            _explodeCts = new CancellationTokenSource();
            DamageExplode(_explodeCts.Token).Forget();
        }
        
        
        private async UniTaskVoid DamageExplode(CancellationToken ct)
        {
            try
            {
                float evaluatedTime = 0f;
                
                while (evaluatedTime < _data.DelayLerpRadius && !ct.IsCancellationRequested)
                {
                    evaluatedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(evaluatedTime / _data.DelayLerpRadius);
                    float currentRadiusValue = Mathf.Lerp(_data.StartLerpRadius, _data.EndLerpRadius, t);
                    
                    CheckDamagebles(currentRadiusValue);
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
                }
                
                CheckDamagebles(_data.EndLerpRadius);
            }
            catch (OperationCanceledException){}
        }

        private void CheckDamagebles(float radius)
        {
            int hitsCount = Physics.SphereCastNonAlloc(transform.position, radius, Vector3.up,
                _hits, radius, _data.DamageLayerMask);
            
            _hitsTransforms.Clear();

            if (hitsCount > 0)
            {
                for (int i = 0; i < hitsCount; i++)
                {
                    if(_hits[i].transform == null || _hits[i].transform == transform || 
                       _hitsTransforms.Contains(_hits[i].transform))continue;

                    if (_hits[i].transform.TryGetComponent(out DamageableComponent damageable))
                    {
                        _hitsTransforms.Add(_hits[i].transform);
                        damageable.TakeDamage(new FlatDamage(_data.DamageCount));
                        //Add fire effect
                    }
                }
            }
        }
        
        public void SwitchState<T>() where T : GranateStateBase
        {
            var newState = _allGranateStates.FirstOrDefault(s=> s is T);
            _currentState?.OnStopState();

            if(newState == null)throw new Exception("New granate state is null");
            newState.OnStartState();
            _currentState = newState;
        }

        public void SwitchState(GranateStateName newState)
        {
            switch (newState)
            {
                case GranateStateName.GRANATE:
                    SwitchState<GranateState>();
                    break;
                case GranateStateName.MINE:
                    SwitchState<MineState>();
                    break;
                default:
                    SwitchState<GranateState>();
                    break;
            }

            _currentStateName = newState;
        }
        
        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_explodeCts != null && !_explodeCts.IsCancellationRequested)
            {
                _explodeCts.Cancel();
                _explodeCts.Dispose();
            }

            foreach (var state in _allGranateStates)
            {
                state.Dispose();
            }
        }
    }
}