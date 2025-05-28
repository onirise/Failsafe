using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    [SerializeField]
    Button _yesButton;


    public void InitialiseWindow(UnityAction funcToListen, string text)
    {
        _yesButton.onClick.AddListener(funcToListen);

    }

    public void CLoseWindow()
    {
        _yesButton.onClick.RemoveAllListeners();
        Destroy(this.gameObject);
    }
}
