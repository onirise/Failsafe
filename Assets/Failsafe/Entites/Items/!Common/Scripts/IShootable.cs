using UnityEngine;

namespace Failsafe.Items
{
    public interface ITargetable
    {
        public void TargetAction(RaycastHit hit);
    }
}
