using UnityEngine;

public class MainMenuSettings : MonoBehaviour
{
    public GameObject graphicsMenu;
    public GameObject soundMenu;
    public GameObject gameplayMenu;




    public void  OnGraphicSettingsClick()
    {   
        Debug.Log("knopka nazhata");
        graphicsMenu.SetActive(true);
        soundMenu.SetActive(false);
        gameplayMenu.SetActive(false);

    }

    public void  OnSoundSettingsClick()
    {   
        Debug.Log("knopka nazhata");
        graphicsMenu.SetActive(false);
        soundMenu.SetActive(true);
        gameplayMenu.SetActive(false);

    }

    public void  OnGameSettingsClick()
    {   
        Debug.Log("knopka nazhata");
        gameplayMenu.SetActive(true);
        graphicsMenu.SetActive(false);
        soundMenu.SetActive(false);
        

    }


}
