using UnityEngine;

public class ElectricalPanelScript : MonoBehaviour
{
    [SerializeField]private PowerSource _powerSource;
    [SerializeField]private bool _isEnable = true;
    private void Start()
    {
        _powerSource.SetEnable(_isEnable);
    }
    private void OnEnablePowerSource()
    {
        _isEnable = !_isEnable;
        _powerSource.SetEnable(_isEnable);
    }

    private void OnMouseDown()
    {
        OnEnablePowerSource();
    }
}
