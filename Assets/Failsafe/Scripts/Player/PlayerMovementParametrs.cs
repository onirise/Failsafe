using System;

/// <summary>
/// Параметры настройки движения игрока
/// </summary>
[Serializable]
public class PlayerMovementParametrs
{
    /// <summary>
    /// Скорость ходьбы
    /// </summary>
    public float walkSpeed = 8f;
    /// <summary>
    /// Скорость бега
    /// </summary>
    public float runSpeed = 15f;
    /// <summary>
    /// Скорость ходьбы в присяди
    /// </summary>
    public float crouchSpeed = 3f;
    /// <summary>
    /// Высота в присяди относительно нормальной высоты
    /// </summary>
    public float crouchHeight = 0.5f;
    /// <summary>
    /// Высота в скольжении относительно нормальной высоты
    /// </summary>
    public float slideHeight = 0.3f;
    /// <summary>
    /// Время скольжения
    /// </summary>
    public float slideTime = 1f;
    /// <summary>
    /// Сила прыжка
    /// </summary>
    public float jumpForce = 10f;
    /// <summary>
    /// Сила угасания прыжка
    /// </summary>
    public float jumpForceFade = 10f;
    /// <summary>
    /// Сила притяжения
    /// </summary>
    public float gravityForce = 8f;
}
