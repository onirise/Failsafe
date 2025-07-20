using UnityEngine;

public class DoorScript: MonoBehaviour
{
    private Animator _animator;
    private Color _defoltColorLight;//это пока UI не прекрутили к двери
    private string _enemyTag = "Enemy";

    private bool _isPowered;
    private bool _enemyBlockDoor = false;
    private bool _doorWasOpen = false;

    [SerializeField] private bool _isOpen;
    [SerializeField] private Light[] _panelLights;// для отображения места для интерактива с дверью


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _defoltColorLight = _panelLights[0].color;
    }
    public void OnPowered()
    {
        Debug.Log("Door power on");
        _isPowered = true;
        foreach (Light light in _panelLights)
            light.enabled = true;
        if (_enemyBlockDoor)
        {
            _isOpen = !_isOpen;
            _animator.SetBool("isOpen", _isOpen);
            Debug.Log("Active Door");
        }
    }
    public void OffPowered()
    {
        Debug.Log("Door power off");
        _isPowered = false;
        foreach (Light light in _panelLights)
            light.enabled = false;
    }
    private void OpenCloseDoor()
    {
        if (!_isPowered) return;
        if (_enemyBlockDoor) return;
        _isOpen = !_isOpen;
        _animator.SetBool("isOpen", _isOpen);
        Debug.Log("Active Door");
    }
    private void OnMouseDown() //для примера вызов открытия/закрытия дверей
    {
        OpenCloseDoor();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == _enemyTag)
        {
            if (!_isOpen)
            {
                Debug.Log(other.gameObject.tag);
                OpenCloseDoor();
            }
            else
            {
                _doorWasOpen = true;
            }
            _enemyBlockDoor = true;
            foreach (Light light in _panelLights)
                light.color = Color.red;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == _enemyTag)
        {
            _enemyBlockDoor = false;
            foreach (Light light in _panelLights)
                light.color = _defoltColorLight;
            if (_isOpen)
            {
                if (_doorWasOpen)
                {
                    _doorWasOpen = false;
                    return;
                }
                Debug.Log(other.gameObject.tag);
                OpenCloseDoor();
            }
        }
    }
}
