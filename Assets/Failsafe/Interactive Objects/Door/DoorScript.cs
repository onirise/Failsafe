using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator _animator;
    private Color _defoltColorLight;//это пока UI не прекрутили к двери
    private string _enemyTag = "Enemy";

    private bool _enemyBlockDoor = false;
    private bool _doorWasOpen = false;

    [SerializeField] private bool _isPowered;
    [SerializeField] private bool _isOpen;
    [SerializeField] private Light[] _panelLights;// для отображения места для интерактива с дверью


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _defoltColorLight = _panelLights[0].color;

        EnableLight(_isPowered);
        _animator.SetBool("isOpen", _isOpen);
    }
    public void OnPowered()
    {
        Debug.Log("Door power on");
        _isPowered = true;
        EnableLight(_isPowered);
        if (_enemyBlockDoor)
        {
            _isOpen = true;
            _animator.SetBool("isOpen", true);
            Debug.Log("Active Door");
        }
    }
    public void OffPowered()
    {
        Debug.Log("Door power off");
        _isPowered = false;
        EnableLight(_isPowered);
    }
    private void OpenCloseDoor(bool open)
    {
        if (!_isPowered) return;
        if (_enemyBlockDoor) return;
        _isOpen = open;
        _animator.SetBool("isOpen", open);
        Debug.Log("Active Door");
    }
    public void InteractDoor()
    {
        OpenCloseDoor(!_isOpen);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(_enemyTag)) return;


        _doorWasOpen = _isOpen;
        OpenCloseDoor(true);
        _enemyBlockDoor = true;
        ChangeColor(Color.red);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(_enemyTag)) return;

        _enemyBlockDoor = false;
        ChangeColor(_defoltColorLight);

        if (_doorWasOpen) { return; }

        OpenCloseDoor(false);
    }
    private void ChangeColor(Color lightColor)
    {
        foreach (Light panelLight in _panelLights)
        {
            panelLight.color = lightColor;
        }
    }
    private void EnableLight (bool doorIsPowered)
    {
        foreach (Light light in _panelLights)
            light.enabled = doorIsPowered;
    }
}