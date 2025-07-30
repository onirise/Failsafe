using UnityEngine;

public class ElectricalPanelScript : Interactable
{
    [SerializeField]private PowerSource _powerSource;
    [SerializeField]private bool _isEnable;
    private void Start()
    {
        _powerSource.SetEnable(_isEnable);
    }
    private void OnEnablePowerSource()
    {
        _isEnable = !_isEnable;
        _powerSource.SetEnable(_isEnable);
    }
    protected override void Interact()
    {
        OnEnablePowerSource();
    }
}
