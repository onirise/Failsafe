using UnityEngine;

public class PowerCrossInteraction : Interactable
{
    private PowerCross _powerCross;
    private void Start()
    {
        _powerCross = GetComponent<PowerCross>();
    }
    protected override void Interact()
    {
        Debug.Log(_powerCross);
        _powerCross.Rotate();
    }
}
