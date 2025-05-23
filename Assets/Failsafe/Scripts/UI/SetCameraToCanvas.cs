using UnityEngine;
using UnityEngine.UI;
public class SetCameraToCanvas : MonoBehaviour
{
    public Canvas canvas;
    void Start()
    {
        canvas.worldCamera = GetComponentInParent<Camera>();
    }

    
}
