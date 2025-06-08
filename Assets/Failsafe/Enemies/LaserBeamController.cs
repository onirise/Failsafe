using TMPro;
using UnityEngine;

public class LaserBeamController : MonoBehaviour
{
    private Transform _origin;
    private Transform _target; // Игрок
    private float _maxLength = 30f;

    public void Initialize(Transform origin, Transform target)
    {
        _origin = origin;
        _target = target;
    }

    private void Update()
    {
        if (_origin == null || _target == null)
            return;

        Vector3 start = _origin.position;
        Vector3 direction = (_target.position - start).normalized;

        float length = _maxLength;

        if (Physics.Raycast(start, direction, out RaycastHit hit, _maxLength))
        {
            length = hit.distance;
        }

        Vector3 mid = start + direction * (length / 2f);

        transform.position = mid;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.localScale = new Vector3(0.1f, 0.1f, length);
    }
}
