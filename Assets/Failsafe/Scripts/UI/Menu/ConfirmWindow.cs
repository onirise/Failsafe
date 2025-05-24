using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    public Button yesButton;
    
  
    public void InitialiseWindow(UnityAction funcToListen, string text) 
    {
        yesButton.onClick.AddListener(funcToListen);
        
    }

    public void CLoseWindow()
    {
        yesButton.onClick.RemoveAllListeners();
        Destroy(this.gameObject);
    }
}
