using UnityEngine;

public class DetectionOnSight : MonoBehaviour
{
    private FieldOfView fov;
    public DetectionProgress detectionProgress;

    private void Start()
    {
        fov = this.transform.parent.gameObject.GetComponent<FieldOfView>();
        if (fov == null)
        {
            Debug.LogError("FieldOfView not found in the scene.");
        }
        else
        {
            Debug.Log("FieldOfView found in the scene.");
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            detectionProgress = other.gameObject.GetComponent<DetectionProgress>();
            fov.FieldOfViewCheck();
            if (detectionProgress != null)
            {
                detectionProgress._playerInNearZone = fov.canSeePlayerNear;
                detectionProgress._playerInFarZone = !fov.canSeePlayerNear && fov.canSeePlayerFar;
            }
        }
            
    }
}

