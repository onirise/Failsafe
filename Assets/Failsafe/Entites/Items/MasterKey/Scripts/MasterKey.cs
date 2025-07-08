using Failsafe.Items;
using UnityEngine;

public class MasterKey : IUsable, ITargetable
{
    public void TargetAction(Ray ray)
    {

        //маска чтобы рейкаст точно игнорировал игрока
        LayerMask mask = ~(1 << 5);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            //Debug.DrawRay(transform.position, transform.up * hit.distance, Color.green);
            Debug.Log("Object ahead: " + hit.collider.name);
            if (hit.collider.GetComponent<DoorScript>() != null)
            {
                hit.collider.GetComponent<DoorScript>().HackDoor();
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.up, Color.red);
            Debug.Log("No Object!");
        }

    }


    public void Use()
    {

    }


}
