using UnityEngine;

public class MaxHealthModifier : MonoBehaviour
{
    public MaxHealthModifierData Data;

    public void ChangeMaxHealth()
    {
        //GetComponentInParent<PlayerHealth>().MaxHealth += Data.MaxHealthDelta;
        //GetComponentInParent<PlayerUniqieMods>().StimpackUsed = true;
    }
}
