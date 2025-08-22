using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент, который вызывает стан врага, при столкновении врага с другими объектами
/// </summary>
public class EnemyPhysicsStunComponent : MonoBehaviour
{
    private Enemy_ScriptableObject _physicsStunData;
    private Collider[] _enemyColliders;
    private Enemy _enemy;
    void Start()
    {
        _enemyColliders = GetComponentsInChildren<Collider>();
        _enemy = GetComponent<Enemy>();
        _physicsStunData = _enemy._enemyConfig;
    }

    void OnCollisionEnter(Collision collision)
    {
        float stunTime = Mathf.Pow(collision.relativeVelocity.magnitude, 2) * collision.rigidbody.mass * _physicsStunData.StunMultiplier;
        if (stunTime > _physicsStunData.MinStunTime)
        {
            stunTime = Mathf.Min(stunTime, _physicsStunData.MaxStunTime);
            _enemy.DisableState(stunTime/1000);
            Debug.Log("Стан: " + stunTime/1000 + "секунд");
        }
    }
}