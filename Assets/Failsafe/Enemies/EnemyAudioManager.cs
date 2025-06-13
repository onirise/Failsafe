using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] private EventReference Step, Run, Reload;
    public void StepEvent()
    {
        SoundUtils3D.Play(this.gameObject, Step);
    }

    public void ReloadEvent()
    {
        SoundUtils3D.Play(this.gameObject, Reload);
    }
    
}
