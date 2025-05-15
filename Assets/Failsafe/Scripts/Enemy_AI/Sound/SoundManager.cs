using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private GameObject soundEmitterPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EmitSound(Vector3 position, SoundData soundData)
    {
        Debug.Log($"Emitting sound at {position} with data: {soundData.soundName}");
        GameObject obj = Instantiate(soundEmitterPrefab, position, Quaternion.identity);
        var emitter = obj.GetComponent<SoundEmitter>();
        emitter.Initialize(soundData);
    }
}