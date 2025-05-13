using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class SoundDetection : MonoBehaviour
{
    [SerializeField] private float baseDetectionRadiusNear = 10f; // Радиус обнаружения звука
    [SerializeField] private float baseDetectionRadiusMedium = 20f; // Угол обнаружения звука
    [SerializeField] private float baseDetectionRadiusFar = 30f; // Радиус обнаружения звука


    private void OnCollisionEnter(Collision collision)
    {
        // Находим всех врагов в радиусе baseDetectionRadiusFar
        Collider[] colliders = Physics.OverlapSphere(transform.position, baseDetectionRadiusFar);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance <= baseDetectionRadiusNear)
                {
                    // Логика для ближней зоны (например, мгновенная реакция)
                    Debug.Log($"Противник {col.name} в БЛИЖНЕЙ зоне! (Расстояние: {distance})");
                }
                else if (distance <= baseDetectionRadiusMedium)
                {
                    // Логика для средней зоны (например, подозрение)
                    Debug.Log($"Противник {col.name} в СРЕДНЕЙ зоне. (Расстояние: {distance})");
                }
                else if (distance <= baseDetectionRadiusFar)
                {
                    // Логика для дальней зоны (например, минимальная реакция)
                    Debug.Log($"Противник {col.name} в ДАЛЬНЕЙ зоне. (Расстояние: {distance})");
                }
            }
        }
    }
}


