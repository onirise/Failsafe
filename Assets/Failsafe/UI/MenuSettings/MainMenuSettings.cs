using UnityEngine;

public class MainMenuSettings : MonoBehaviour
{
    public GameObject graphicsMenu;
    public GameObject soundMenu;
    public GameObject gameplayMenu;

    


    public void  OnGraphicSettingsClick()
    {   
        Debug.Log("Settings button pressed");
        graphicsMenu.SetActive(true);
        soundMenu.SetActive(false);
        gameplayMenu.SetActive(false);

    }

    public void  OnSoundSettingsClick()
    {   
        Debug.Log("Settings button pressed");
        graphicsMenu.SetActive(false);
        soundMenu.SetActive(true);
        gameplayMenu.SetActive(false);

    }

    public void  OnGameSettingsClick()
    {   
        Debug.Log("Settings button pressed");
        gameplayMenu.SetActive(true);
        graphicsMenu.SetActive(false);
        soundMenu.SetActive(false);
        

    }


}
