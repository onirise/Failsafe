using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    private SoundData _data;
    private float _elapsedTime = 0f;

    public void Initialize(SoundData data)
    {
        _data = data;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        float t = _elapsedTime / _data.duration;
        float currentRadius = Mathf.Lerp(_data.maxRadius, 0f, t);

        NotifyListeners(currentRadius);

        if (_elapsedTime >= _data.duration)
            Destroy(gameObject);
    }

    private void NotifyListeners(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in hits)
        {
            IHearSound listener = col.GetComponent<IHearSound>();
            if (listener != null)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                listener.OnSoundHeard(transform.position, _data, distance);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_data == null) return;

        float t = Application.isPlaying ? _elapsedTime / _data.duration : 0f;
        float currentRadius = Mathf.Lerp(_data.maxRadius, 0f, t);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}