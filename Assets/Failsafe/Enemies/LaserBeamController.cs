using UnityEngine;

public class LaserBeamController : MonoBehaviour
{
    private Transform _origin;
    private Transform _target; // Теперь это Transform, чтобы следить за точкой на теле
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
        Vector3 end = _target.position;
        Vector3 direction = (end - start).normalized;

        float distance = Vector3.Distance(start, end);
        float length = Mathf.Min(distance, _maxLength);

        // Проверка на столкновение
        if (Physics.Raycast(start, direction, out RaycastHit hit, length))
        {
            length = hit.distance;
        }

        // Центр лазера посередине между началом и концом
        Vector3 mid = start + direction * (length / 2f);

        transform.position = mid;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.localScale = new Vector3(0.1f, 0.1f, length);
    }
}
