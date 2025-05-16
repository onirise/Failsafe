using UnityEngine;
using Zenject;

public abstract class BaseConfirmCallButton : MonoBehaviour
{
    public string textToView;

    [Inject] TabletHandler menuHandler;

    public void CallConfirm()
    {
        
        ConfirmWindow newConfirmWindow = Instantiate(Resources.Load<ConfirmWindow>("ConfirmWindow"), menuHandler.canvasParent);
        newConfirmWindow.InitialiseWindow(this,new Vector3(0.5f,0.5f, 1));
    }

    public abstract void funcToListen();
}
