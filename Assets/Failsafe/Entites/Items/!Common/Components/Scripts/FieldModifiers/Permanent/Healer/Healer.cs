using UnityEngine;

public class Healer : MonoBehaviour
{
    public HealData Data;

    public void Heal()
    {
        //GetComponentInParent<PlayerHealth>().Health += HealData.HealAmount;
        Debug.Log("Healed");
    }
}
