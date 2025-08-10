using UnityEngine.UI;
using VContainer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction: MonoBehaviour
{
    [SerializeField] private Camera _playerCam;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _mask;

    [Inject]
    private InputHandler _inputHandler;

    [Header("Crosshair")]
    [SerializeField] private Image _crosshairImage;
    [SerializeField] private float _hoverSize = 0.6f;
    [SerializeField] private float _scaleSpeed = 8f;

    private float _normalSize;

    void Start()
    {
        _normalSize = _crosshairImage.transform.localScale.x;
    }

    
    void Update()
    {
        float targetScale = _normalSize;

        Ray ray = new Ray(_playerCam.transform.position, _playerCam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, _distance, _mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (_inputHandler.GrabOrDropAction.WasPressedThisFrame()) //использовал триггер GrapOrDrop так как не смг создать свой
                {
                    interactable.BaseInteract();
                }
                targetScale = _hoverSize;
            }
        }
        UpdateCrosshairScale(targetScale);
    }
    private void UpdateCrosshairScale(float scale)
    {
        float current = _crosshairImage.rectTransform.localScale.x;
        float next = Mathf.Lerp(current, scale, Time.deltaTime * _scaleSpeed);
        _crosshairImage.rectTransform.localScale = new Vector3(next, next, 1f);
    }
}
