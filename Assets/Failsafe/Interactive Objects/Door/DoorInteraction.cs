using UnityEngine;

public class DoorInteraction : Interactable
{
    [SerializeField] private DoorScript _doorScript;
    protected override void Intaract()
    {
        _doorScript.InteractDoor();
    }
}
