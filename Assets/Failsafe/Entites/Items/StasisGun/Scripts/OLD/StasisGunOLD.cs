using System;
using UnityEngine;

[RequireComponent(typeof(EnergyContainerOLD))]
public class StasisGunOLD : MonoBehaviour
{
    [SerializeField] EnergyContainerOLD _energyContainer1;
    public StasisGunData Data;
    private bool _isDefaultMode = true;
    float _fireRateTimer = 0;
    private void Update()
    {
        //пока что это затычка, ибо не понимаю как это сделать с помощью inputHandler и ActionGroups в Item
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            _isDefaultMode = !_isDefaultMode;
            Debug.Log("Default mode is " + _isDefaultMode);
        }
        if (_fireRateTimer > 0)
        {
            _fireRateTimer -= Time.deltaTime;
        }
    }

    public void ChangeMode()
    {
        _isDefaultMode = !_isDefaultMode;
        Debug.Log("Default mode is " + _isDefaultMode);
    }

    public void Shoot(RaycastHit hit)
    {
        if (_fireRateTimer <= 0 && !_energyContainer1.IsEmpty())
        {
            _fireRateTimer = Data.FireRate;
            _energyContainer1.UseChargeAmount();
            if (_isDefaultMode)
                DefaultMode(hit);
            else
                AltMode(hit);
        }
    }

    void DefaultMode(RaycastHit hit)
    {
        if (hit.collider.GetComponent<Stasisable>() != null)
        {
            hit.collider.GetComponent<Stasisable>().StartStasis(Data.StasisDuration);
        }
        else if (hit.collider.GetComponentInParent<Enemy>() != null)
        {
            hit.collider.GetComponentInParent<Enemy>().DisableState(Data.StasisDuration);
        }
    }

    void AltMode(RaycastHit hit)
    {
        if (hit.collider.GetComponent<Stasisable>() != null)
        {
            hit.collider.GetComponent<Stasisable>().StartStasisWithInertion(Data.StasisDuration);
        }
        else if (hit.collider.GetComponentInParent<Enemy>() != null)
        {
            hit.collider.GetComponentInParent<Enemy>().DisableState(Data.StasisDuration);
        }
    }
}
