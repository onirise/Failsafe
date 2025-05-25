using UnityEngine;
using Zenject;

public class OpenSaveLoadWindow : MonoBehaviour
{
    [SerializeField]
    SaveState saveStateInButton;
    [Inject] GameplaySavesHandler gameplaySavesHandler;
    [Inject] ProfilesHandler profilesHandler;
    public void OpenWindow()
    {
        //gameplaySavesHandler.OpenGSavesWindow(profilesHandler.GetSelectedProfile(), saveStateInButton);
    }
}
