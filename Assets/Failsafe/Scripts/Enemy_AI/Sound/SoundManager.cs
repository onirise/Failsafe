using System;
using UnityEngine;

public class SoundManager
{
    public static Action<SoundData> OnSoundEmitted;

    public static void EmitSound(SoundData soundData)
    {
        OnSoundEmitted?.Invoke(soundData);
    }
}
