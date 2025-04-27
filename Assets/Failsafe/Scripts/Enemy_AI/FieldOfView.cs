using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void  FieldOfViewCheck()
    {
        canSeePlayerFar = false;
        canSeePlayerNear = false;

        if (CheckVisibility(radiusNear, angleNear))
        {
            canSeePlayerNear = true;
            Debug.Log("Player detected");
        }
        // Проверка в дали
        if (CheckVisibility(radiusFar, angleFar) && !canSeePlayerNear)
        {
            canSeePlayerFar = true;
            Debug.Log("Player detected");
        }
        // Проверка вблизи
       

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
                Debug.Log("Player is in sight");
                return true;
            }
        }

        return false;
    }
}








