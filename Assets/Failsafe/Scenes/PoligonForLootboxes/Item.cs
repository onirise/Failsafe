using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Debug.Log($"magnitude: {GetComponent<Rigidbody>().linearVelocity.magnitude}");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        PickUp();
    }

    public virtual bool PickUp()
    {
        // if (!GetComponent<Rigidbody>().IsSleeping()) return false;
        if (GetComponent<Rigidbody>().linearVelocity.magnitude > 2.0f)
        {
            return false;
        }

        Destroy(gameObject);
        return true;
    }
}
