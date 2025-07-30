using UnityEngine;

namespace Failsafe.Items
{
    [CreateAssetMenu(fileName = "AdrenalineData", menuName = "ScriptableObjects/Entities/Items/Components/AdrenalineData")]
    public class AdrenalineData : ScriptableObject
    {
        public float SpeedMultiplier;
        public float Duration;
    }
}
