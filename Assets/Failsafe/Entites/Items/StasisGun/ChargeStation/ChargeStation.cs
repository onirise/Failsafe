using System;
using UnityEngine;

public class ChargeStation : MonoBehaviour
{
    [SerializeField] Transform _posForPistolGO;
    [SerializeField] int _containedEnergyAmount = 2000;

    private void OnTriggerEnter(Collider other)
    {
        EnergyContainerOLD connectedEnergyContainer = other.GetComponent<EnergyContainerOLD>();
        //other.transform.position = _posForPistolGO.position;
        //other.GetComponent<Rigidbody>().isKinematic = true;
        if (connectedEnergyContainer != null)
        {
            int energyAmountOfConnectedObj = connectedEnergyContainer.GetAmountForMax();
            if (_containedEnergyAmount >= energyAmountOfConnectedObj && !connectedEnergyContainer.IsFull())
            {
                connectedEnergyContainer.Reload(energyAmountOfConnectedObj);
                _containedEnergyAmount -= energyAmountOfConnectedObj;
                if (_containedEnergyAmount == 0)
                    Destroy(this.gameObject);
            }

        }
    }


}
