using UnityEngine;

public class ElevatorParentController : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Elevator collision enter: {collision}");
        collision.transform.SetParent(transform);
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log($"Elevator collision exit: {collision}");
        collision.transform.SetParent(null);
    }
}
