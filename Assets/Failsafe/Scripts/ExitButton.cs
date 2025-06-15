using Failsafe.UI.MainMenu.Popup;
using UnityEngine;

public class ExitButton : MonoBehaviour
{

    [SerializeField] public Popup popup;
    public void OnClick()
    {
        popup.Show();
    }
    public void ExitGame()
    {
        # if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}