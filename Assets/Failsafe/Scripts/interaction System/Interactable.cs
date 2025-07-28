using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        //функция которую будут переопределять подклассы
    }
}
