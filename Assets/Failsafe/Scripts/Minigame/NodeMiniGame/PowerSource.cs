// Начальная точка
using UnityEngine;

public class PowerSource : PowerNode
{
    private bool _isEnable = true;
    [SerializeField]
    private ElectricalPanelScript _electricalPanel;

    private void Start()
    {
        // Например, питание запускается автоматически или после починки
        if (_electricalPanel != null) return;
        StartPower();
    }
    public void SetEnable(bool isEnable)
    {
        _isEnable = isEnable;
        var manager = FindFirstObjectByType<PowerNetworkManager>();
        if (manager != null)
        {
            manager.RefreshPower();
        }
        else
        {
            Debug.LogWarning("PowerNetworkManager not found in scene.");
        }
    }
    public override void StartPower()
    {
        if (!_isEnable) return;
        base.StartPower();
    }
}
