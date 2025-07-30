using UnityEngine;

namespace Failsafe.Items
{
    [CreateAssetMenu(fileName = "GorillaData", menuName = "ScriptableObjects/Entities/Items/GorillaData")]
    public class GorillaData : ScriptableObject
    {
        public float ThrowPowerMultiplier;
        public float Duration;
    }
}
