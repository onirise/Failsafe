using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference walk;
    [SerializeField] private FMODUnity.EventReference run;
    [SerializeField] private FMODUnity.EventReference reload;


    public void StepEvent()
    {
        SoundUtils3D.Play(this.gameObject, walk);
    }

    public void RunStep()
    {
        SoundUtils3D.Play(this.gameObject, run);
    }

    public void ReloadEvent()
    {
        SoundUtils3D.Play(this.gameObject, reload);
    }
}
