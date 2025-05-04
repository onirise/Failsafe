using FMOD;
using UnityEngine;

public class SoundEmittTrigger : MonoBehaviour
{
    [SerializeField] private SoundType soundType = SoundType.Impact;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private bool emitOnStart = false;
    public void Emit()
    {
        SoundData data = new SoundData(soundType, radius, duration);
        SoundManager.Instance.EmitSound(transform.position, data);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Пример: если объект упал и коснулся земли
        Emit();
    }
}
