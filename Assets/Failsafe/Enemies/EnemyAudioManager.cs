using FMODUnity;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference walk;
    [SerializeField] private FMODUnity.EventReference run;
    [SerializeField] private FMODUnity.EventReference reload;


    public void StepEvent()
    {
        RuntimeManager.PlayOneShot(walk, transform.position);
    }

    public void RunStep()
    {
        RuntimeManager.PlayOneShot(run, transform.position);
    }

    public void ReloadEvent()
    {
        RuntimeManager.PlayOneShot(reload, transform.position);
    }
}
