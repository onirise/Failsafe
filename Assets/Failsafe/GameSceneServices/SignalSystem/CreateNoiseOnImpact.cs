using UnityEngine;

/// <summary>
/// Компонент, который создает сигнал шума при столкновении объекта с другими объектами
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CreateNoiseOnImpact : MonoBehaviour
{
    private const float MinImpactValue = 0.1f;
    private SignalChannel _noiseChanel => SignalManager.Instance.PlayerNoiseChanel;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            var impact = CalculateNoise(collision, contact);
            if (impact < MinImpactValue) continue;
            Debug.Log("Impact = " + impact);
            _noiseChanel.Add(contact.point, impact, 5);
        }
    }

    private float CalculateNoise(Collision collision, ContactPoint contactPoint)
    {
        // TODO: По этой формуле получаются большие цифры, нужно балансить
        // Добавить в расчет материалы предметов
        var impact = Vector3.Dot(contactPoint.normal, collision.relativeVelocity) * _rigidbody.mass;
        return impact / 10f;
    }
}
