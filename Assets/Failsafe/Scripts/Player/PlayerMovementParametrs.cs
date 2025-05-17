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
    /// Скорость скольжения
    /// </summary>
    public float slideSpeed = 12f;
    /// <summary>
    /// Высота в скольжении относительно нормальной высоты
    /// </summary>
    public float slideHeight = 0.3f;
    /// <summary>
    /// Максимальное время скольжения
    /// </summary>
    public float maxSlideTime = 1f;
    /// <summary>
    /// Минимальное время скольжения
    /// </summary>
    public float minSlideTime = 0.5f;
    /// <summary>
    /// Сила прыжка
    /// </summary>
    public float jumpForce = 20f;
    /// <summary>
    /// Сила угасания прыжка
    /// </summary>
    public float jumpForceFade = 30f;
    /// <summary>
    /// Сила притяжения
    /// </summary>
    public float gravityForce = 8f;
}
