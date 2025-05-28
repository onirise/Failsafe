using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class TabletHandler : MonoBehaviour
{
    //[SerializeField]
    //Canvas _canvas;


    void Start()
    {
        //SaveManager.profilesHandler = this;
        PauseManager.Pause(false);
        GetComponentInChildren<Canvas>().worldCamera = GetComponentInParent<Camera>();
        //_canvas.worldCamera = GetComponentInParent<Camera>();
        SaveManager.LoadAll();

    }

}

