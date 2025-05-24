using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class TabletHandler : MonoBehaviour
{
      
    public Transform canvasParent;

    public GameObject tabletGO;    

    public GameObject MainMenuGO;
    public GameObject GameplayMenuGO;
    public float time;

    public PlayButton playButton;

    //[Inject] SaveManager saveManager;
    [Inject] ProfilesHandler profilesHandler;
    [Inject] GameplaySavesHandler gameplaySavesHandler;
    
    void Start()
    {
       
        SaveManager.profilesHandler = profilesHandler;
        PauseManager.Pause(false);
        //SetCursor();
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            
            EnableTablet(true);
            
        }
        else
            EnableTablet(false);
        SaveManager.LoadAll();
        Profile currentProfile = profilesHandler.GetSelectedProfile();
        if(currentProfile==null)
            playButton.SetPlayButtonInteractable(false);
        else
        {
             gameplaySavesHandler.profileParent = currentProfile;
             gameplaySavesHandler.SetSavesFromSelectedProfile(currentProfile.DATA.gameplaySaveDATAs);
             if(currentProfile.DATA.isNew)
            {
                currentProfile.DATA.isNew = false;
                gameplaySavesHandler.gameplaySaves[0].OnSaveGameplaySave();
            }
        }
       
       
            
        

        
    }
    void Update()
    {
        
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)) && !(SceneManager.GetActiveScene().name == "MainMenu"))
        {
            EnableTablet(!tabletGO.activeSelf);
           
        }
        if(!PauseManager.CheckPause())
            time += Time.deltaTime;

    }

    public void EnableTablet(bool flag)
    {
        tabletGO.SetActive(flag);
        SetCursor();
        PauseManager.Pause(tabletGO.activeSelf);

        
        gameplaySavesHandler.gameplaySavesGO.SetActive(false);

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

    void SetCursor()
    {
        Cursor.visible = tabletGO.activeSelf;
        Cursor.lockState = Cursor.visible? CursorLockMode.None : CursorLockMode.Locked;
    }

    
   
}

