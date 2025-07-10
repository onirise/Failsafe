using Failsafe.Items;
using UnityEngine;

namespace Failsafe.Items
{
    public class MasterKey : IUsable, ITargetable
    {
        public void TargetAction(RaycastHit hit)
        {
            if (hit.collider.GetComponent<DoorScript>() != null)
            {
                hit.collider.GetComponent<DoorScript>().HackDoor();
            }
        }


        public void Use()
        {

        }


    }
}
