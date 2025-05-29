using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMenu : BaseMenu
{

    public void OnBackToGame()
    {
        GetComponentInParent<TabletActivator>().EnableTablet(false);
    }

    public void OnSaveGame()
    {
        Debug.Log("SavesOpenedForSave");
        //КОД ИЗ OpenSaveLoadWindow

        //SaveState _saveStateInButton;
        //[Inject] GameplaySavesHandler gameplaySavesHandler;
        //[Inject] ProfilesHandler profilesHandler;
        //public void OpenWindow()
        //{
        //gameplaySavesHandler.OpenGSavesWindow(profilesHandler.GetSelectedProfile(), saveStateInButton);
        //}
    }


    public void OnLoadGame()
    {
        Debug.Log("SavesOpenedForLoad");
    }
}
