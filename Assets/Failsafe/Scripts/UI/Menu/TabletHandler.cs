using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class TabletHandler : MonoBehaviour
{
      
    public Transform canvasParent;

    public GameObject tabletGO;    

    [Inject] SaveManager saveManager;
    
 
    public GameObject MainMenuGO;
    public GameObject GameplayMenuGO;
    public float time;
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            EnableTablet();
        }
        saveManager.LoadAll();
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            EnableTablet();
           
        }
        if(!PauseManager.CheckPause())
            time += Time.deltaTime;

    }

    public void EnableTablet()
    {
        tabletGO.SetActive(!tabletGO.activeSelf);
        Cursor.visible = tabletGO.activeSelf;
        Cursor.lockState = Cursor.visible? CursorLockMode.None : CursorLockMode.Locked;
        PauseManager.Pause(tabletGO.activeSelf);

        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            MainMenuGO.SetActive(true);
            GameplayMenuGO.SetActive(false);
        }
        else
        {
            MainMenuGO.SetActive(false);
            GameplayMenuGO.SetActive(true);
        }
            
    }

   
   
}

