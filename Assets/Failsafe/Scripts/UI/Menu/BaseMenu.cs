using UnityEngine;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField]
    ConfirmWindow confirmWindowPrefab;
   

 

    [Inject] TabletHandler menuHandler;
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevelWithConfirm(string levelName)
    {
         CallConfirm(() => 
        {
            LoadLevel(levelName);
        });
       
    }

    public void CallConfirm(UnityAction funcToListen, string text = "Are you sure")
    {
        
        ConfirmWindow newConfirmWindow = Instantiate(confirmWindowPrefab, menuHandler.canvasParent);
        //newConfirmWindow.yesButton.onClick.AddListener(funcToListen);
        newConfirmWindow.InitialiseWindow(funcToListen, text);
    }
    
}
