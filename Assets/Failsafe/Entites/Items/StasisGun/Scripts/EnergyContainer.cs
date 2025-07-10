using UnityEngine;

public class EnergyContainer
{
    StasisGunData _data;
    [SerializeField] int _energyAmountCurrent;

    public EnergyContainer(StasisGunData data)
    {
        _data = data;
        _energyAmountCurrent = data.ChargeAmountMax;
    }

    public void Reload(int amount)
    {
        _energyAmountCurrent += amount;
        _energyAmountCurrent = Mathf.Clamp(_energyAmountCurrent, 0, _data.ChargeAmountMax);
    }

    public void UseChargeAmount()
    {
        _energyAmountCurrent -= 1;
        _energyAmountCurrent = Mathf.Clamp(_energyAmountCurrent, 0, _data.ChargeAmountMax);
        Debug.Log("energy amoun = " + _energyAmountCurrent);
    }

    public bool IsFull()
    {
        return _energyAmountCurrent == _data.ChargeAmountMax;
    }

    public bool IsEmpty()
    {
        return _energyAmountCurrent == 0;
    }

    public int GetAmountForMax()
    {
        return _data.ChargeAmountMax - _energyAmountCurrent;
    }
}
