using System;
using UnityEngine;

public class EnemySoundListener : MonoBehaviour
{
    private enum ReactionType { Near, Medium, Far } // Fixed: Changed to a proper enum declaration

    [SerializeField] private float hearingMultiplier = 1.0f;

    private void OnEnable()
    {
        SoundManager.OnSoundEmitted += OnSoundHeard;
    }

    private void OnDisable()
    {
        SoundManager.OnSoundEmitted -= OnSoundHeard;
    }

    private void OnSoundHeard(SoundData sound)
    {
        float effectiveRadius = sound.radius * hearingMultiplier;
        float distance = Vector3.Distance(transform.position, sound.position);

        if (distance > effectiveRadius)
            return;

        Debug.Log($"{gameObject.name} heard a {sound.type} at {sound.position}");

        switch (sound.type)
        {
            case SoundType.Footstep:
                SuspiciousLook(sound.position);
                break;
            case SoundType.Impact:
                Investigate(sound.position);
                break;
            case SoundType.Distract:
                MoveToDistractPoint(sound.position);
                break;
            case SoundType.Explosion:
                Alert(sound.position);
                break;
        }
    }

    private void SuspiciousLook(Vector3 pos) => Debug.Log($"{name} is suspicious near {pos}");
    private void Investigate(Vector3 pos) => Debug.Log($"{name} is investigating {pos}");
    private void MoveToDistractPoint(Vector3 pos) => Debug.Log($"{name} is distracted and moves to {pos}");
    private void Alert(Vector3 pos) => Debug.Log($"{name} is alert and running to {pos}");
}
