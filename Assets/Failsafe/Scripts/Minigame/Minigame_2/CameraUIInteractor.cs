using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiCanvasCameraInteractor : MonoBehaviour
{
    [SerializeField]private Camera _cam;
    [SerializeField]private float _maxDistance = 10f;

    private EventSystem _eventSystem;
    private PointerEventData _pointerEventData;
    // Список всех Canvas с GraphicRaycaster в сцене
    private List<GraphicRaycaster> _graphicRaycasters = new List<GraphicRaycaster>();

    void Start()
    {
        _eventSystem = EventSystem.current;
        if (_eventSystem == null)
        {
            Debug.LogError("EventSystem отсутствует в сцене!");
            return;
        }

        // Находим все Canvas с GraphicRaycaster
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (var canvas in canvases)
        {
            var graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster != null && canvas.renderMode == RenderMode.WorldSpace)
            {
                _graphicRaycasters.Add(graphicRaycaster);
            }
        }
    }

    void Update()
    {
        if (_graphicRaycasters.Count == 0) return;

        _pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        List<RaycastResult> allResults = new List<RaycastResult>();

        // Проходим по всем графическим Raycaster'ам и собираем результаты
        foreach (var gr in _graphicRaycasters)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(_pointerEventData, results);
            allResults.AddRange(results);
        }

        if (allResults.Count > 0)
        {
            // Сортируем по distance (чем меньше — тем ближе к камере)
            allResults.Sort((r1, r2) => r1.distance.CompareTo(r2.distance));

            RaycastResult topResult = allResults[0];
            GameObject hitUI = topResult.gameObject;

            //Debug.Log($"Навели на UI элемент: {hitUI.name} (Canvas: {topResult.module.gameObject.name})");

            if (Input.GetButtonDown("Fire1"))
            {
                ExecuteEvents.Execute(hitUI, _pointerEventData, ExecuteEvents.pointerClickHandler);
                Debug.Log($"Click {hitUI}");
            }
        }
    }
}
