using FMODUnity;
using UnityEngine;

public static class SoundUtils3D
{
    public static void Play(GameObject target, EventReference eventReference)
    {
        var emitter = target.GetComponent<StudioEventEmitter>();
        if (emitter != null)
        {
            // Если другой EventReference — установить новый
            if (emitter.EventReference.Guid != eventReference.Guid)
            {
                emitter.EventReference = eventReference;
            }

            emitter.Play();
        }
        else
        {
            Debug.LogWarning($"[SoundUtils] Play failed - StudioEventEmitter not found on {target.name}");
        }
    }

    public static void Stop(GameObject target)
    {
        var emitter = target.GetComponent<StudioEventEmitter>();
        if (emitter != null)
        {
            emitter.Stop();
        }
        else
        {
            Debug.LogWarning($"[SoundUtils] Stop failed - StudioEventEmitter not found on {target.name}");
        }
    }

    public static void SetParameter(GameObject target, string parameterName, float value)
    {
        var emitter = target.GetComponent<StudioEventEmitter>();
        if (emitter != null && emitter.EventInstance.isValid())
        {
            emitter.EventInstance.setParameterByName(parameterName, value);
        }
        else
        {
            Debug.LogWarning($"[SoundUtils] SetParameter failed - StudioEventEmitter not found or invalid on {target.name}");
        }
    }

    public static void SetVolume(GameObject target, float volume)
    {
        var emitter = target.GetComponent<StudioEventEmitter>();
        if (emitter != null && emitter.EventInstance.isValid())
        {
            emitter.EventInstance.setVolume(volume);
        }
        else
        {
            Debug.LogWarning($"[SoundUtils] SetVolume failed - StudioEventEmitter not found or invalid on {target.name}");
        }
    }
}