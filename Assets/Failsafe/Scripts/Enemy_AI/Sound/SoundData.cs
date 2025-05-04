using UnityEngine;

public enum SoundType { Footstep, Impact, Distract, Explosion }

[System.Serializable]
public class SoundData
{
    public SoundType soundType;
    public float maxRadius;
    public float duration;

    public SoundData(SoundType type, float radius, float time)
    {
        soundType = type;
        maxRadius = radius;
        duration = time;
    }
}