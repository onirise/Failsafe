using UnityEngine;

public class PowerNetworkManager : MonoBehaviour
{
    private PowerNode[] _allNodes;

    private void Awake()
    {
        _allNodes = FindObjectsOfType<PowerNode>();
    }

    // Перезапуск питания: сбросить у всех, потом запустить у источников
    public void RefreshPower()
    {
        // Сброс питания у всех
        foreach (var node in _allNodes)
        {
            node.ResetPower();
        }

        // Запуск питания от всех источников
        foreach (var node in _allNodes)
        {
            if (node is PowerSource source)
            {
                source.StartPower();
            }
        }

        // После запуска питания можно проверить, кто не получил питание и отключить их явно,
        // но так как ResetPower и ReceivePower управляют состоянием, это не обязательно.
    }
}