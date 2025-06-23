using System;
using UnityEngine;

/// <summary>
/// Менеджер всех сигналов на уровне
/// </summary>
public class SignalManager : MonoBehaviour
{
    [Obsolete("Заменить на DI контейнер")]
    public static SignalManager Instance { get; private set; }

    /// <summary>
    /// Канал шума, издаваемый игроком или объектами
    /// </summary>
    public SignalChannel PlayerNoiseChanel = new SignalChannel(() => TempNoiseSignal.Zero);
    /// <summary>
    /// Канал шума, издаваемый врагами
    /// </summary>
    public SignalChannel EnemyNoiseChanel = new SignalChannel(() => TempNoiseSignal.Zero);

    private float _lastRemoveExpireAt;
    private const float _removeExpireDelay = 5;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        var currentTime = Time.time;

        PlayerNoiseChanel.DecaySignals(Time.deltaTime);
        EnemyNoiseChanel.DecaySignals(Time.deltaTime);

        if (_lastRemoveExpireAt + _removeExpireDelay > currentTime) return;

        PlayerNoiseChanel.RemoveExpiredSignals(currentTime);
        EnemyNoiseChanel.RemoveExpiredSignals(currentTime);
        _lastRemoveExpireAt = currentTime;
    }

    void OnDrawGizmos()
    {
        if (PlayerNoiseChanel == null) return;
        foreach (var signal in PlayerNoiseChanel.GetAllActive())
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(signal.SourcePosition, signal.SignalStrength);
        }
    }

    [ContextMenu("Debug Player Channel")]
    public void DebugPlayerChannel()
    {
        DebugChannel(PlayerNoiseChanel);
    }

    private void DebugChannel(SignalChannel channel)
    {
        var list = channel.GetAllActive();
        Debug.Log($"CurrentTime = {Time.time}; Signals count = {list.Count}");
        for (int i = 0; i < list.Count; i++)
        {
            ISignal signal = list[i];
            Debug.Log($"[{i}] {signal}");
        }
    }
}
