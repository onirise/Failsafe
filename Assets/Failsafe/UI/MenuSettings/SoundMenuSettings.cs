using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SoundMenuSettings : MonoBehaviour
{
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public GameObject saveButton;
    public GameObject defaultButton;


    public void OnMasterVolumeChange()
    {

        Debug.Log(masterVolume.value);



    }

    public void OnMusicVolumeChange()
    {

        Debug.Log(musicVolume.value);



    }
    public void OnSFXVolumeChange()
    {

        Debug.Log(sfxVolume.value);



    }
    public void  OnSaveButtonClick()
    {   
        Debug.Log("Settings button pressed");
        

    }

    public void  OnDefaultButtonClick()
    {   
        Debug.Log("Settings button pressed");


    }




}
