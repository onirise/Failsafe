using UnityEngine;
using VContainer;

public class ZoomEffect : MonoBehaviour
{
    private bool zooming = false;
    public int zoom = 50;
    private float normal;
    private float smooth = 5f;
    [SerializeField] private Camera m_Camera;
    [Inject] private InputHandler _inputHandler;

    private void Awake()
    {
        if (m_Camera == null)
        {
            m_Camera = Camera.main; // запасной вариант
        }

        normal = m_Camera.fieldOfView; // сохраняем обычное поле зрения
    }

    void Update()
    {
        // Считаем "зажата ли кнопка" прямо в апдейте
        zooming = _inputHandler.ZoomTriggered;

        if (zooming)
        {
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, zoom, Time.deltaTime * smooth);
        }
        else
        {
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, normal, Time.deltaTime * smooth);
        }
    }
}
