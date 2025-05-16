using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitToMainMenuButton : BaseConfirmCallButton
{
   
     public override void funcToListen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
