using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    public SpeedEffectData Data;

    public void AddBuff()
    {
        //var speedEffect = GetComponentInParent<PlayerController>().gameObject.AddComponent<SpeedEffect>();
        //speedEffect.Data = Data;
        Debug.Log("Added buff");
    }
}
