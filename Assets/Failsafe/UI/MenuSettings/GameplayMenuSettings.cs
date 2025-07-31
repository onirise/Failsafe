using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayMenuSettings : MonoBehaviour
{
    public Slider sensetivityLevel;
    public Slider cameraShakeLevel;
    public GameObject saveButton;
    public GameObject defaultButton;

    public void OnSensetivityLevelChange()
    {

        Debug.Log(sensetivityLevel.value);



    }

    public void OnCamerShakeChange()
    {

        Debug.Log(cameraShakeLevel.value);



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
