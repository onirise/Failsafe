using UnityEngine;

namespace Failsafe.Items
{
    [CreateAssetMenu(fileName = "TushkanData", menuName = "ScriptableObjects/Entities/Items/TushkanData")]
    public class TushkanData : ScriptableObject
    {
        public float JumpMultiplier;
        public float Duration;
    }
}
