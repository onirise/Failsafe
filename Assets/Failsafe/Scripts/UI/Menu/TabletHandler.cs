using UnityEngine;



public class TabletHandler : MonoBehaviour
{
    [SerializeField]
    Canvas _canvas;


    void Start()
    {
        //SaveManager.profilesHandler = this;
        PauseManager.Pause(false);
        _canvas.worldCamera = GetComponentInParent<Camera>();
        //_canvas.worldCamera = GetComponentInParent<Camera>();


    }

}

