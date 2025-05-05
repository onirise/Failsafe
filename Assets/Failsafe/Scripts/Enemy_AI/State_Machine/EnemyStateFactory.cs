using System;
using UnityEngine;

public class EnemyStateFactory
{
    public static EnemyBaseState CreateState(EnemyStateType stateType)
    {
        switch (stateType)
        {
            case EnemyStateType.Patrol:
                return new EnemyPatrolingState();
            case EnemyStateType.Chase:
                return new EnemyChaseState();
            case EnemyStateType.Attack:
                return new EnemyAttackState();
            case EnemyStateType.Search:
                return new EnemySearchState();
            default:
                throw new Exception("Unknown state type");
        }
    }
}