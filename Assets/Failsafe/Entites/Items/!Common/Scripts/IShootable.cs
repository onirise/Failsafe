using UnityEngine;

namespace Failsafe.Items
{
    public interface IShootable
    {
        public void Shoot(RaycastHit hit);
    }
}