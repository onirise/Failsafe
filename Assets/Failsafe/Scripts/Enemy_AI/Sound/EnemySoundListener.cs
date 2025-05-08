using FMOD;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemySoundListener : MonoBehaviour, IHearSound
{
    private enum ReactionType { Near, Medium, Far } // Fixed: Changed to a proper enum declaration

    public void OnSoundHeard(Vector3 soundPosition, SoundData data, float distance)
    {
        switch (data.soundType)
        {
            case SoundType.Footstep:
                if (distance < data.maxRadius * 0.3f)
                {
                    Debug.Log($"{name} услышал шаги и насторожился.");
                    // Переход в state Alert, например
                }
                break;

            case SoundType.Explosion:
                Debug.Log($"{name} услышал упавший предмет и идет проверять.");
                // Перейти в состояние Search
                break;

            case SoundType.Distract:
                Debug.Log($"{name} услышал громкий звук — тревога!");
                // Немедленный переход в агрессивное состояние
                break;

            case SoundType.Impact:
                Debug.Log($"{name} отвлекся на шум.");
                Investigate(soundPosition);
                // Перейти к точке звука
                break;
        }
    }
    private void SuspiciousLook(Vector3 pos) => Debug.Log($"{name} is suspicious near {pos}");
    private void Investigate(Vector3 pos)
    {
        Debug.Log($"{name} отвлекся на шум.");
        var stateMachine = GetComponent<EnemyStateMachine>();

        if (stateMachine.CurrentState is not EnemyChaseState)
        {
            stateMachine.searchingPoint = pos;
            stateMachine.SwitchState<EnemySearchState>();
        }
    }
    private void MoveToDistractPoint(Vector3 pos) => Debug.Log($"{name} is distracted and moves to {pos}");
    private void Alert(Vector3 pos) => Debug.Log($"{name} is alert and running to {pos}");

   
}
