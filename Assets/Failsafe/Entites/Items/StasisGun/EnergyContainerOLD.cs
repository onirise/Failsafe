using UnityEngine;

public class EnergyContainerOLD : MonoBehaviour
{
    public StasisGunData Data;
    [SerializeField] int _energyAmountCurrent;

    private void Start()
    {
        _energyAmountCurrent = Data.ChargeAmountMax;
    }

    public void Reload(int amount)
    {
        _energyAmountCurrent += amount;
        _energyAmountCurrent = Mathf.Clamp(_energyAmountCurrent, 0, Data.ChargeAmountMax);
    }

    public void UseChargeAmount()
    {
        _energyAmountCurrent -= 1;
        _energyAmountCurrent = Mathf.Clamp(_energyAmountCurrent, 0, Data.ChargeAmountMax);
    }

    public bool IsFull()
    {
        return _energyAmountCurrent == Data.ChargeAmountMax;
    }

    public bool IsEmpty()
    {
        return _energyAmountCurrent == 0;
    }

    public int GetAmountForMax()
    {
        return Data.ChargeAmountMax - _energyAmountCurrent;
    }
}
