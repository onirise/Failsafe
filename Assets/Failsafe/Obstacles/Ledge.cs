using UnityEngine;

/// <summary>
/// Выступ на который можно взбираться
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class Ledge : MonoBehaviour
{
    public const float MinLedgeSize = 0.2f;
    public const float MaxLedgeSize = float.MaxValue;

    void OnValidate()
    {
        if (transform.localScale.x < MinLedgeSize || transform.localScale.y < MinLedgeSize || transform.localScale.z < MinLedgeSize)
        {
            transform.localScale = new Vector3(
                Mathf.Clamp(transform.localScale.x, MinLedgeSize, MaxLedgeSize),
                Mathf.Clamp(transform.localScale.y, MinLedgeSize, MaxLedgeSize),
                Mathf.Clamp(transform.localScale.z, MinLedgeSize, MaxLedgeSize)
            );
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
