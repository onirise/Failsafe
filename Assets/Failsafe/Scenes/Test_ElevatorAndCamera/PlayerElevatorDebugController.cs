using UnityEngine;

// Данный скрипт предназначен только для дебага лифта
public class PlayerElevatorDebugController : MonoBehaviour
{
    [SerializeField] ElevatorController _elevator;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _elevator.OnButtonDownPress();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _elevator.OnButtonUpPress();
        }
    }
}
