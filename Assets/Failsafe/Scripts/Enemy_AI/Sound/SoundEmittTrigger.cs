using FMOD;
using UnityEngine;

public class SoundEmittTrigger : MonoBehaviour
{
    [SerializeField] private DataContainer.SpreadSheetContainer dataContainer;
    [SerializeField] private string soundName = "test1"; // Название звука
    [SerializeField] private SoundType soundType = SoundType.Impact;
    [SerializeField] private float radius;
    [SerializeField] private float duration;

    private void Awake()
    {
        var data = dataContainer.Content.soundDatas.Find(x => x.soundName == soundName);
        if (data != null)
        {
            soundName = data.soundName;
            soundType = data.soundType;
            radius = data.maxRadius;
            duration = data.duration;
        }
        else
        {
            UnityEngine.Debug.Log($"Sound data with name {soundName} not found.");
        }
    }

    public void Emit()
    {
        SoundData data = new SoundData();
        SoundManager.Instance.EmitSound(transform.position, data);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Пример: если объект упал и коснулся земли
        Emit();
    }
}
