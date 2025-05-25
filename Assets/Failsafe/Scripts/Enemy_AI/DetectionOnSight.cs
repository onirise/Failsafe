using UnityEngine;

public class DetectionOnSight : MonoBehaviour
{
    private FieldOfView fov;
    public DetectionProgress detectionProgress;

    private void Start()
    {
        fov = transform.parent.GetComponent<FieldOfView>();
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
                // Определение зоны с приоритетом ближней
                detectionProgress.CurrentZone = fov.canSeePlayerNear ? DetectionProgress.DetectionZone.Near :
                                              fov.canSeePlayerFar ? DetectionProgress.DetectionZone.Far :
                                              DetectionProgress.DetectionZone.None;
            }
        }
            
    }
}

