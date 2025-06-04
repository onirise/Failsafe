using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    [Header("Радиусы окружностей поля зрения")]
    public float radiusFar;
    public float radiusNear;
    public bool canSeePlayerFar, canSeePlayerNear;
   [SerializeField] GameObject collisionFar, collisionNear;
    [Range(0, 360)]
    public float angleFar, angleNear;
    public GameObject playerRef;
    [SerializeField] private LayerMask targetMask, obstructionMask;

    private void Start()
    {
        if(collisionFar != null)
        {
            collisionFar.GetComponent<SphereCollider>().radius = radiusFar;
        }
        if (collisionNear != null)
        {
            collisionNear.GetComponent<SphereCollider>().radius = radiusNear;
        }
    }

    /// <summary>
    /// Проверяет видимость игрока в разных радиусах и углах.
    /// </summary>
    public bool FieldOfViewCheck()
    {
        canSeePlayerFar = false;
        canSeePlayerNear = false;

        // Проверяем ближнюю зону
        if (CheckVisibility(radiusNear, angleNear))
        {
            canSeePlayerNear = true;
            Debug.Log("Player detected in NEAR range");
        }
        // Проверяем дальнюю зону только если не обнаружено в ближней
        else if (CheckVisibility(radiusFar, angleFar))
        {
            canSeePlayerFar = true;
            Debug.Log("Player detected in FAR range");
        }

        // Возвращаем true если обнаружен в любой зоне
        return canSeePlayerNear || canSeePlayerFar;

    }

    /// <summary>
    /// Проверяет, виден ли игрок в заданном радиусе и угле.
    /// </summary>
    /// <param name="radius">Радиус проверки.</param>
    /// <param name="angle">Угол проверки.</param>
    /// <returns>Возвращает true, если игрок виден.</returns>
    private bool CheckVisibility(float radius, float angle)
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length == 0)
            return false;

        Transform target = rangeChecks[0].transform;
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                RotateTowardsPlayer(target.position); // Поворачиваем врага к игроку
                if(target.GetComponent<DetectionProgress>().InChase)
                {
                    this.GetComponent<EnemyStateMachine>().SwitchState<EnemyChaseState>();
                }
                Debug.Log("Player is in sight");
                return true;
            }
        }

        return false;
    }

    public void RotateTowardsPlayer(Vector3 playerPosition)
    {
        // Вычисляем направление к игроку (игнорируем разницу по высоте)
        Vector3 direction = playerPosition - transform.position;
        direction.y = 0; // Оставляем только горизонтальный поворот

        if (direction != Vector3.zero)
        {
            // Создаем целевой поворот
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Плавно поворачиваем с учетом скорости
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * gameObject.GetComponent<NavMeshAgent>().angularSpeed // Добавьте public float rotationSpeed в ваш класс
            );
        }
    }
}








