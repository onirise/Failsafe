using Failsafe.Scripts.Health;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private Slider HealthSlider;

    private IHealth _health;

    [Inject]
    public void Construct(IHealth health)
    {
        _health = health;
    }

    private void Update()
    {
        HealthText.text = $"{(int)_health.CurrentHealth} / {_health.MaxHealth}";
        HealthSlider.value = _health.CurrentHealth / _health.MaxHealth;
    }
}