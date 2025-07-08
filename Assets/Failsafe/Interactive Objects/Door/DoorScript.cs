using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DoorScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private bool _isPowered;
    [SerializeField] private bool _isHardClosed = true;
    [SerializeField] private bool _isHacked;
    [SerializeField] private bool _isOpen;


    [SerializeField] private Light[] _panelLights;// для отображения места для интерактива с дверью

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void OnPowered()
    {
        Debug.Log("Door power on");
        _isPowered = true;
        foreach (Light light in _panelLights)
            light.enabled = true;

        if (!_isHacked) SwitchLightColor(Color.red);

    }
    public void OffPowered()
    {
        Debug.Log("Door power off");
        _isPowered = false;
        foreach (Light light in _panelLights)
            light.enabled = false;
    }

    void SwitchLightColor(Color newColor)
    {
        foreach (Light light in _panelLights)
            light.color = newColor;
    }
    private void OpenCloseDoor()
    {
        if (_isHacked)
        {
            _isOpen = !_isOpen;
            _animator.SetBool("isOpen", _isOpen);
            Debug.Log("Active Door");
        }
        else
            Debug.Log("Door Closed!");

    }
    private void OnMouseDown() //для примера вызов открытия/закрытия дверей
    {
        if (_isPowered)
            OpenCloseDoor();
    }

    public void HackDoor()
    {
        if (_isPowered)
        {
            _isHacked = true;
            SwitchLightColor(Color.green);
        }

    }
}
