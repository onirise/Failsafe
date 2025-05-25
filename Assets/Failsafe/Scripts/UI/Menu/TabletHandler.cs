using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class TabletHandler : MonoBehaviour
{

    public Transform canvasParent;

    public GameObject tabletGO;


    public float time;

    [Inject] ProfilesHandler profilesHandler;
    [Inject] GameplaySavesHandler gameplaySavesHandler;

    void Start()
    {
        SaveManager.profilesHandler = profilesHandler;
        PauseManager.Pause(false);
        SaveManager.LoadAll();

    }
    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)) && !(SceneManager.GetActiveScene().name == "MainMenu"))
        {
            EnableTablet(!tabletGO.activeSelf);

        }
        if (!PauseManager.CheckPause())
            time += Time.deltaTime;

    }

    public void EnableTablet(bool flag)
    {
        tabletGO.SetActive(flag);
        SetCursor();
        PauseManager.Pause(tabletGO.activeSelf);
        gameplaySavesHandler.gameplaySavesGO.SetActive(false);
    }

    void SetCursor()
    {
        Cursor.visible = tabletGO.activeSelf;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }



}

