using System;
using UnityEngine;

namespace Failsafe.Scripts.Damage.Implementation
{
    [Serializable]
    public class FlatDamage : IDamage
    {
        [field: SerializeField]
        public float DamageAmount { get; private set; }

        public FlatDamage(float damageAmount)
        {
            DamageAmount = damageAmount;
        }
    }
}