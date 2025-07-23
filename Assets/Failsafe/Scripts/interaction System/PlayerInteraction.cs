using Failsafe.Player;
using UnityEngine;

public class PlayerInteraction: MonoBehaviour
{
    [SerializeField] private Camera _playerCam;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _mask;

    void Start()
    {

    }

    void Update()
    {
        Ray ray = new Ray(_playerCam.transform.position, _playerCam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, _distance, _mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (Input.GetButtonDown("Fire1"))
                {
                    interactable.BaseInteract();
                }
            }
        }

    }
}
