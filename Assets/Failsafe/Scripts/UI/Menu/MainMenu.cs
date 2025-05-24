using UnityEngine;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : BaseMenu
{
     [SerializeField]
    Button playButton;

    public GameObject profilesMainGO;
   
    [Inject] ProfilesHandler profilesHandler;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetPlayButtonInteractable(bool flag)
    {
        playButton.interactable = flag; 
    }

    public void ExitGame()
    {
        CallConfirm(() => 
        {
            Debug.LogWarning("Quit");
            Application.Quit();
        });
        
    }

    public void OnProfilesOpen()
    {
        gameObject.SetActive(false);
        profilesMainGO.SetActive(true);
    } 

    private void OnEnable() 
    {
        playButton.interactable = profilesHandler.profiles.Count > 0;
        Debug.Log("профили 1 " + profilesHandler.profiles.Count);
    }

    

   
}
