using TMPro;
using UnityEngine;

public class LaserBeamController : MonoBehaviour
{
    private Transform _origin;
    private Vector3 _direction;
    private float _maxLength = 30f;

    public void Initialize(Transform origin, Vector3 targetPosition)
    {
        _origin = origin;
        _direction = (targetPosition - origin.position).normalized;
    }

    private void Update()
    {
        if (_origin == null)
            return;

        Vector3 start = _origin.position;
        Vector3 dir = _direction;

        float length = _maxLength;

        if (Physics.Raycast(start, dir, out RaycastHit hit, _maxLength))
        {
            length = hit.distance;
        }

        Vector3 mid = start + dir * (length / 2f);

        transform.position = mid;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.localScale = new Vector3(0.1f, 0.1f, length);
    }
}
