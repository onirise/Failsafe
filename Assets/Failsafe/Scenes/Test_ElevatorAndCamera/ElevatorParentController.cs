using UnityEngine;

public class ElevatorParentController : MonoBehaviour
{
    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log($"Elevator collision enter: {collision}");
    //     collision.transform.SetParent(transform);
    // }

    // void OnCollisionExit(Collision collision)
    // {
    //     Debug.Log($"Elevator collision exit: {collision}");
    //     collision.transform.SetParent(null);
    // }

    void OnTriggerEnter(Collider other)
    {
        // if (other.GetComponent<Player>() == null) return;
        Debug.Log($"Elevator other enter: {other}");
        other.transform.SetParent(transform);
    }

    void OnTriggerExit(Collider other)
    {
        // if (other.GetComponent<Player>() == null) return;
        Debug.Log($"Elevator other exit: {other}");
        other.transform.SetParent(null);
    }
}
