using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    public Button yesButton;
    
    public void InitialiseWindow(BaseConfirmCallButton _button, Vector3 scale) 
    {
        yesButton.onClick.AddListener(_button.funcToListen);
        transform.localScale = scale;
    }

    public void CLoseWindow()
    {
        yesButton.onClick.RemoveAllListeners();
        Destroy(this.gameObject);
    }
}
