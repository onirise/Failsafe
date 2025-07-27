using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public void BaseInteract()
    {
        Intaract();
    }
    protected virtual void Intaract()
    {
        //функция которую будут переопределять подклассы
    }
}
