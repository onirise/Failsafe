using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Failsafe.Obstacles;
using Unity.AI.Navigation; // Добавляем using для доступа к Ledge

/// <summary>
/// Генерирует NavMeshLink для подъема, используя существующие в сцене объекты Ledge.
/// Находит все компоненты Ledge, берет их активные края и создает линки от пола снизу до центра края.
/// </summary>
[ExecuteAlways]
public class OffMeshLinkAutoBuilder : MonoBehaviour
{
    [Header("Настройки Поиска")]
    [Tooltip("Слой, на котором находится геометрия пола (нижний уровень).")]
    [SerializeField]
    private LayerMask floorMask = 1;

    [Tooltip("Максимальное расстояние от края уступа вниз до пола.")] [SerializeField]
    private float maxClimbHeight = 5.0f;

    [Tooltip("Минимальное расстояние от края уступа вниз до пола.")]
    [SerializeField] private float minClimbHeight = 0.5f;

    [Tooltip("Расстояние между создаваемыми линками на одном уступе.")]
    [SerializeField] private float linkSpacing = 2.0f;

    [Tooltip("Насколько близко могут быть конечные точки разных линков.")]
    [SerializeField] private float linkProximity = 1.0f;

    [Header("Параметры NavMeshLink")] [SerializeField]
    private float linkWidth = 0.8f;

    [SerializeField] private int area = 0; // 0 = Walkable
    [SerializeField] private float costModifier = 1.0f;

    [Header("Отладка")] [Tooltip("Включить отображение гизмо для созданных линков.")]
    [SerializeField] private bool visualizeProcess = true;
    [Tooltip("Включить РАСШИРЕННУЮ отладку. Показывает лучи и точки проверки. Используйте для проблемных уступов.")]
    [SerializeField] private bool enableDeepDebug = false;

    private readonly List<NavMeshLink> _createdLinks = new List<NavMeshLink>();
#if UNITY_EDITOR
    private readonly List<System.Tuple<Vector3, Vector3>> _debug_createdLinkPoints = new List<System.Tuple<Vector3, Vector3>>();
    // Для расширенной отладки
    private readonly List<System.Tuple<Vector3, Vector3, Color>> _debug_rays = new List<System.Tuple<Vector3, Vector3, Color>>();
    private readonly List<System.Tuple<Vector3, Color>> _debug_samplePoints = new List<System.Tuple<Vector3, Color>>();
    private readonly List<LedgeEdge> _debug_processedEdges = new List<LedgeEdge>();
#endif

    [ContextMenu("Generate Links From Ledges")]
    public void GenerateLinks()
    {
        ClearLinks();

        Ledge[] allLedges = FindObjectsOfType<Ledge>();
        if (allLedges == null || allLedges.Length == 0)
        {
            Debug.LogWarning("[LedgeLinkGenerator] В сцене не найдено ни одного объекта с компонентом 'Ledge'.", this);
            return;
        }

        foreach (Ledge ledge in allLedges)
        {
            // Принудительно инициализируем края, если это еще не сделано
            ledge.Awake();

            var edges = new List<LedgeEdge>();
            if (ledge.FrontEdge != null) edges.Add(ledge.FrontEdge);
            if (ledge.BackEdge != null) edges.Add(ledge.BackEdge);
            if (ledge.LeftEdge != null) edges.Add(ledge.LeftEdge);
            if (ledge.RightEdge != null) edges.Add(ledge.RightEdge);

            foreach (LedgeEdge edge in edges)
            {
#if UNITY_EDITOR
                if (enableDeepDebug) _debug_processedEdges.Add(edge);
#endif
                ProcessEdge(edge);
            }
        }

        Debug.Log($"[LedgeLinkGenerator] Поиск завершен. Создано {_createdLinks.Count} линков для подъема.", this);
    }

    private void ProcessEdge(LedgeEdge edge)
    {
        float edgeLength = Vector3.Distance(edge.Point1, edge.Point2);
        // Создаем как минимум один линк, даже если край короткий
        int linkCount = Mathf.Max(1, Mathf.RoundToInt(edgeLength / linkSpacing));

        for (int i = 0; i < linkCount; i++)
        {
            // Равномерно распределяем точки вдоль края. Если линк один, он будет в центре.
            float t = (linkCount <= 1) ? 0.5f : (float)i / (linkCount - 1);
            Vector3 topPoint = Vector3.Lerp(edge.Point1, edge.Point2, t);

            // Стреляем лучом из точки на краю вниз, чтобы найти пол
            if (Physics.Raycast(topPoint, Vector3.down, out RaycastHit floorHit, maxClimbHeight, floorMask))
            {
                float climbHeight = Vector3.Distance(topPoint, floorHit.point);
                if (climbHeight < minClimbHeight)
                {
#if UNITY_EDITOR
                    if (enableDeepDebug) _debug_rays.Add(new System.Tuple<Vector3, Vector3, Color>(topPoint, floorHit.point, Color.yellow));
#endif
                    continue; // Пропускаем эту точку, если она не подходит по высоте
                }

#if UNITY_EDITOR
                if (enableDeepDebug) _debug_rays.Add(new System.Tuple<Vector3, Vector3, Color>(topPoint, floorHit.point, Color.green));
#endif

                // Проверяем наличие NavMesh в обеих точках
                bool topOk = NavMesh.SamplePosition(topPoint, out NavMeshHit topNavHit, 2.0f, NavMesh.AllAreas);
                bool bottomOk = NavMesh.SamplePosition(floorHit.point, out NavMeshHit bottomNavHit, 2.0f, NavMesh.AllAreas);

#if UNITY_EDITOR
                if (enableDeepDebug)
                {
                    _debug_samplePoints.Add(new System.Tuple<Vector3, Color>(topPoint, topOk ? Color.green : Color.red));
                    _debug_samplePoints.Add(new System.Tuple<Vector3, Color>(floorHit.point, bottomOk ? Color.green : Color.red));
                }
#endif

                if (topOk && bottomOk)
                {
                    CreateLink(bottomNavHit.position, topNavHit.position);
                }
            }
            else
            {
#if UNITY_EDITOR
                if (enableDeepDebug) _debug_rays.Add(new System.Tuple<Vector3, Vector3, Color>(topPoint, topPoint + Vector3.down * maxClimbHeight, Color.red));
#endif
            }
        }
    }

    private void CreateLink(Vector3 start, Vector3 end)
    {
        // Проверяем, нет ли уже линка со слишком близкой конечной точкой
        foreach (var link in _createdLinks)
        {
            if (Vector3.Distance(link.transform.TransformPoint(link.endPoint), end) < linkProximity)
                return;
        }

        var go = new GameObject("LedgeClimbUpLink");
        go.transform.position = start;
        go.transform.SetParent(transform, true);

        var navLink = go.AddComponent<NavMeshLink>();
        navLink.startPoint = Vector3.zero;
        navLink.endPoint = go.transform.InverseTransformPoint(end);
        navLink.width = linkWidth;
        navLink.costModifier = costModifier;
        navLink.area = area;
        navLink.bidirectional = false; // Линки для подъема всегда однонаправленные

        _createdLinks.Add(navLink);
#if UNITY_EDITOR
        if (visualizeProcess) _debug_createdLinkPoints.Add(new System.Tuple<Vector3, Vector3>(start, end));
#endif
    }

    [ContextMenu("Clear Generated Links")]
    public void ClearLinks()
    {
        for (int i = _createdLinks.Count - 1; i >= 0; i--)
        {
            if (_createdLinks[i] != null)
            {
                if (Application.isPlaying) Destroy(_createdLinks[i].gameObject);
                else DestroyImmediate(_createdLinks[i].gameObject);
            }
        }

        _createdLinks.Clear();
#if UNITY_EDITOR
        _debug_createdLinkPoints.Clear();
        _debug_rays.Clear();
        _debug_samplePoints.Clear();
        _debug_processedEdges.Clear();
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!visualizeProcess) return;

        // Основная визуализация созданных линков
        foreach (var points in _debug_createdLinkPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(points.Item1, points.Item2);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(points.Item1, 0.15f); // Start point
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(points.Item2, 0.15f); // End point
        }

        // Расширенная отладка
        if (enableDeepDebug)
        {
            // Рисуем края, которые обрабатывает скрипт
            foreach (var edge in _debug_processedEdges)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(edge.Point1, edge.Point2);
                Gizmos.DrawSphere(edge.Point1, 0.2f);
                Gizmos.DrawSphere(edge.Point2, 0.2f);
            }

            foreach (var ray in _debug_rays)
            {
                Gizmos.color = ray.Item3;
                Gizmos.DrawLine(ray.Item1, ray.Item2);
            }
            foreach (var point in _debug_samplePoints)
            {
                Gizmos.color = point.Item2;
                Gizmos.DrawSphere(point.Item1, 0.1f);
            }
        }
    }
#endif
}