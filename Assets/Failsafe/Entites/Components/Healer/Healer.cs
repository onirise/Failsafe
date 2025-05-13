using UnityEngine;

public class Healer : MonoBehaviour
{
    public HealData HealData;

    public void Heal()
    {
        //GetComponentInParent<PlayerHealth>().Health += HealData.HealAmount;
    }
}
