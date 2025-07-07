using System;
using UnityEngine;

public class PropThrower : MonoBehaviour
{
    [SerializeField] float _throwForce = 10f; // Сила броска
    [SerializeField] GameObject _throwingObject;
    private Rigidbody rb;

    private void Start()
    {
        rb = _throwingObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float angle = 60f * Mathf.Deg2Rad; 
        Vector3 forceDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        Debug.DrawRay(transform.position, forceDirection, Color.red);
        if (Input.GetKeyDown(KeyCode.J))
        {
            _throwingObject.transform.position = transform.position;
            rb.AddForce(forceDirection * _throwForce, ForceMode.Impulse);
        }
    }
}
