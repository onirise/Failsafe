using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFromGameButton : BaseConfirmCallButton
{
    

    public override void funcToListen()
    {
        Debug.LogWarning("Quit");
        Application.Quit();
    }
}
