using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Failsafe.Granate
{
    [Serializable]
    public class GranateData
    {
        [field: SerializeField] public float DelayLerpRadius { get; private set; }
        [field: SerializeField] public float StartLerpRadius { get; private set; }
        [field: SerializeField] public float EndLerpRadius { get; private set; }
        [field: SerializeField] public LayerMask DamageLayerMask { get; private set; }
        [field: SerializeField] public int HitsCount { get; private set; }
        [field: SerializeField] public int DamageCount { get; private set; }
        
    }
}