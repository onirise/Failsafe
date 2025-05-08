using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Collider))] // Требуем наличие коллайдера
public class SoundEmittTrigger : MonoBehaviour
{
    [Header("Sound Configuration")]
    [SerializeField] private DataContainer.SpreadSheetContainer dataContainer;
    [SerializeField] private string soundName = "test1";

    [Header("Emission Settings")]
    [SerializeField] private bool emitOnCollision = true;
    [SerializeField] private LayerMask collisionMask = ~0; // Все слои по умолчанию
    [SerializeField] private float minCollisionForce = 0.5f;

    private SoundData _cachedSoundData;
    private bool _isInitialized;

    private void Awake()
    {
        InitializeSoundData();
    }

    private void InitializeSoundData()
    {
        if (dataContainer == null || dataContainer.Content == null)
        {
            Debug.LogError("DataContainer not assigned!", this);
            return;
        }

        var soundData = dataContainer.Content.soundDatas.Find(x => x.soundName == soundName);
        if (soundData == null)
        {
            Debug.LogError($"Sound data '{soundName}' not found in DataContainer!", this);
            return;
        }

        // Кэшируем данные звука
        _cachedSoundData = new SoundData
        {
            soundName = soundData.soundName,
            soundType = soundData.soundType,
            maxRadius = soundData.maxRadius,
            duration = soundData.duration,
            // Добавляем другие параметры при необходимости
        };

        _isInitialized = true;
        Debug.Log($"Sound data initialized: {_cachedSoundData.soundName}", this);
    }

    public void Emit()
    {
        if (!_isInitialized)
        {
            Debug.LogWarning("Sound data not initialized - cannot emit sound", this);
            return;
        }

        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager instance not found!", this);
            return;
        }

        Debug.Log($"Emitting sound: {_cachedSoundData.soundName} at {transform.position}", this);
        SoundManager.Instance.EmitSound(transform.position, _cachedSoundData);
    }

    public void EmitWithOverride(SoundData overrideData)
    {
        if (overrideData == null)
        {
            Emit();
            return;
        }

        Debug.Log($"Emitting overridden sound: {overrideData.soundName}", this);
        SoundManager.Instance.EmitSound(transform.position, overrideData);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!emitOnCollision || !_isInitialized)
            return;

        // Проверяем слой объекта
        if ((collisionMask.value & (1 << collision.gameObject.layer)) == 0)
            return;

        // Проверяем силу удара
        if (collision.relativeVelocity.magnitude < minCollisionForce)
            return;

        Emit();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (dataContainer != null && !string.IsNullOrEmpty(soundName))
        {
            // Автоматически обновляем параметры в редакторе
            var data = dataContainer.Content?.soundDatas.Find(x => x.soundName == soundName);
            if (data != null)
            {
                soundName = data.soundName;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
    }
#endif
}
