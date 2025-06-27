using UnityEngine;
using Failsafe.Scripts.Health;
using VContainer;
using Sirenix.OdinInspector;

public class Healer : MonoBehaviour
{
    public HealData Data;
    [Inject, ShowInInspector, ReadOnly] private IHealth _health;

    public void Heal() =>
        _health.AddHealth(Data.HealAmount);
}
