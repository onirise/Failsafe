using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Радиусы окружностей поля зрения")]
    public float radius;
    public float radiusWalking;
    public float radiusSprinting;
    [Range(0, 360)]
    public float angleSprint, angleWalk, angleNear;
    public GameObject playerRef;
    public LayerMask targetMask, obstructionMask;
    public bool canSeePlayer;



    private void Update()
    {
        FieldOfViewCheck();
    }

    /// <summary>
    /// Проверяет видимость игрока в разных радиусах и углах.
    /// </summary>
    private void FieldOfViewCheck()
    {
        canSeePlayer = false;

        // Проверка для спринта
        if (CheckVisibility(radiusSprinting, angleSprint))
        {
            canSeePlayer = true;
            return;
        }

        // Проверка для ходьбы
        if (CheckVisibility(radiusWalking, angleWalk))
        {
            canSeePlayer = true;
            return;
        }

        // Проверка для ближнего радиуса
        if (CheckVisibility(radius, angleNear))
        {
            canSeePlayer = true;
        }
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
                return true;
            }
        }

        return false;
    }
}








