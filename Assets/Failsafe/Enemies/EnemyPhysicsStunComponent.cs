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
        var stunTime = Mathf.Pow(collision.relativeVelocity.magnitude, 2) * collision.rigidbody.mass 
            * _physicsStunData.StunMultiplier;
        if (stunTime > _physicsStunData.MinStunTime)
        {
            stunTime = Mathf.Min(stunTime, _physicsStunData.MaxStunTime);
            //TODO: Передавать время стана как аргумент в DisabledState()
            _enemy.DisableState();
            Debug.Log("Стан: " + stunTime + "мс");
        }
    }
}