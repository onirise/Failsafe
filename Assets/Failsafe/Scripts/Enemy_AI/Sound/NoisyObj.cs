using UnityEngine;

public class NoisyObj : MonoBehaviour
{
    [SerializeField] private float soundRadius = 10f;
    [SerializeField] private SoundType soundType = SoundType.Impact;

    private void OnCollisionEnter(Collision collision)
    {
        SoundData sound = new SoundData(transform.position, soundRadius, soundType, gameObject);
        SoundManager.EmitSound(sound);
        Debug.Log($"{gameObject.name} emitted sound: {soundType}");
    }
}

