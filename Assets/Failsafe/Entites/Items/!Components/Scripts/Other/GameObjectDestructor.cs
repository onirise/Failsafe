using System;
using UnityEngine;

public class GameObjectDestructor : MonoBehaviour
{
    /// <summary>
    /// Удаляет GameObject
    /// Имеет задержку перед удалением 3 секунды
    /// </summary>
    public void DestroyGO()
    {
        Destroy(gameObject, 3f);
    }

    /// <summary>
    /// Удаляет GameObject
    /// </summary>
    /// <param name="delay">Задержка перед удалением</param>
    public void DestroyGO(float delay)
    {
        Destroy(gameObject, delay);
    }
}
