using Failsafe.Player.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PlayerStaminaUI : MonoBehaviour
{
    [SerializeField] private Slider StaminaSlider;

    private IStamina _stamina;

    [Inject]
    public void Construct(IStamina Stamina)
    {
        _stamina = Stamina;
    }

    private void Update()
    {
        StaminaSlider.value = Mathf.Max(0, _stamina.CurrentStamina) / _stamina.MaxStamina;
    }
}