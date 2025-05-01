using UnityEngine;

public enum SoundType { Footstep, Impact, Distract, Explosion }

public class SoundData
{
    public Vector3 position;
    public float radius;
    public SoundType type;
    public GameObject source;

    public SoundData(Vector3 pos, float rad, SoundType type, GameObject source)
    {
        position = pos;
        radius = rad;
        this.type = type;
        this.source = source;
    }
}