using System;
using UnityEngine;
using UnityEngine.Events;


public class Shooter : MonoBehaviour
{
    
    //public UnityAction<RaycastHit> OnShoot;
    public UnityEvent<RaycastHit> OnShoot;
   
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up*10, Color.yellow);
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit))
        {
            Debug.DrawRay(transform.position, transform.up * hit.distance, Color.green);
            Debug.Log("Object ahead: " + hit.collider.name);
            OnShoot?.Invoke(hit);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up, Color.red);
            Debug.Log("No Object!" );
        }
        
    }

}
