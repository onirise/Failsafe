using UnityEngine;

public interface IHearSound
{
    void OnSoundHeard(Vector3 soundPosition, SoundData data, float distanceToSound);
}