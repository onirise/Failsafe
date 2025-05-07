using UnityEngine;

public class DebugEffect : BaseEffect
{
    public string DebugText;
    public override void Apply()
    {
        Debug.Log(DebugText);
    }
}