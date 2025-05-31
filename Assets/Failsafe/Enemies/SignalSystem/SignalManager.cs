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
    public SignalChannel PlayerNoiseChanel;
    /// <summary>
    /// Канал шума, издаваемый врагами
    /// </summary>
    public SignalChannel EnemyNoiseChanel;

    private float _lastRemoveExpireAt;
    private const float RemoveExpireDelay = 5;

    void Awake()
    {
        PlayerNoiseChanel = new SignalChannel(() => TempNoiseSignal.Zero);
        EnemyNoiseChanel = new SignalChannel(() => TempNoiseSignal.Zero);
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        var currentTime = Time.time;
        if (_lastRemoveExpireAt + RemoveExpireDelay > currentTime) return;

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
