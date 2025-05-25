using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class LightDetector : MonoBehaviour
{
    [Header("Настройки обнаружения")]
    [SerializeField] private Transform[] _detectionPoints; // Точки на теле игрока
    [SerializeField] private LayerMask _lightLayer; // Слой источников света
    [SerializeField] private LayerMask _obstructionLayer; // Слой препятствий
    [SerializeField] private LayerMask _ignoreLayer; // Слой игрока (игнорируется)
    [SerializeField] PlayerVisibility _playerVis; // Ссылка на скрипт обнаружения
    [Header("Отладка")]
    [SerializeField] private bool _visualize = true;
    [SerializeField][Range(0, 1)] private float _illuminationLevel;

    private Collider _detectorCollider;
    private HashSet<Collider> _activeLights = new HashSet<Collider>();

    public float IlluminationLevel => _illuminationLevel;

    private void Awake()
    {
        _detectorCollider = GetComponent<Collider>();
        _detectorCollider.isTrigger = true;

        Debug.Log($"[Освещение] Детектор готов. Точек обнаружения: {_detectionPoints?.Length ?? 0}");
    }

    private void OnTriggerEnter(Collider other)
    { 

        if (!IsValidLight(other)) return;

        _activeLights.Add(other);
        UpdateIllumination();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsValidLight(other)) return;
        UpdateIllumination();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidLight(other)) return;

        _activeLights.Remove(other);
        _playerVis.ResetVisibility();
        UpdateIllumination();
    }

    private bool IsValidLight(Collider col)
    {
        if (col == null) return false;

        // Проверяем тег и слой через встроенные методы Unity
        return col.CompareTag("Light");
    }

    private void UpdateIllumination()
    {
        if (_activeLights.Count == 0 || _detectionPoints == null)
        {
            _illuminationLevel = 0f;
            return;
        }

        int visibleHits = 0;
        int totalChecks = _detectionPoints.Length * _activeLights.Count;
        LayerMask raycastMask = _obstructionLayer & ~_ignoreLayer;

        foreach (var point in _detectionPoints)
        {
            if (point == null) continue;

            foreach (var lightCollider in _activeLights)
            {
                if (lightCollider == null) continue;

                Vector3 direction = lightCollider.transform.position - point.position;
                if (!Physics.Raycast(point.position, direction.normalized, direction.magnitude, raycastMask))
                {
                    visibleHits++;

                    if (_visualize)
                        Debug.DrawLine(point.position, lightCollider.transform.position, Color.green, 0.1f);
                }
                else if (_visualize)
                {
                    Debug.DrawLine(point.position, lightCollider.transform.position, Color.red, 0.1f);
                }
            }
        }

        _illuminationLevel = totalChecks > 0 ? (float)visibleHits / totalChecks : 0f;
        _playerVis.PlayerVisScore = _illuminationLevel * 100;

        Debug.Log($"[Освещение] Статистика: " +
                 $"Точек: {_detectionPoints.Length}, " +
                 $"Источников: {_activeLights.Count}, " +
                 $"Попаданий: {visibleHits}/{totalChecks}, " +
                 $"Освещенность: {_illuminationLevel:P0}");
    }

    private void OnDrawGizmosSelected()
    {
        if (_detectionPoints == null) return;

        Gizmos.color = Color.cyan;
        foreach (var point in _detectionPoints)
        {
            if (point != null)
                Gizmos.DrawWireSphere(point.position, 0.1f);
        }
    }
}