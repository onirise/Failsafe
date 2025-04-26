using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyBaseState currentState;
    public String stateName;
    public EnemySearchState searchState = new EnemySearchState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyPatrolingState patrolState = new EnemyPatrolingState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public GameObject player;
    public GameObject enemyWeapon;
    public float normalSpeed = 3f; // Normal speed of the enemy
    [Header("Enemy Patrol Settings")]
    public float patrolSpeed = 3f;
    public float waitTime = 2f;
    public GameObject[] patrolPoints;
    [Header("Enemy Chase Settings")]
    public float lostPlayerTimer = 5f;
    public float enemyChaseSpeed = 8f;
    public bool afterChase = false; // Flag to check if the enemy is after the player
    [Header("Enemy Search Settings")]
    public float changePointTimer = 0.5f;
    public float searchDuration = 5f; // Duration to search for the player
    public float searchRadius = 10f; // Radius to search for the player
    public float timeToGet = 25f; // Time to get to the search point
    public float offsetSearchinPoint = 15f; // Radius of the search area

    void Start()
    {

        currentState = patrolState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        stateName = currentState.ToString();
        currentState.UpdateState(this);
    }

   public void EnemySwitchState(EnemyBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void LookForPlayer()
    {
        if (this.GetComponent<FieldOfView>().canSeePlayer)
        {
            currentState = chaseState;
            currentState.EnterState(this);
        }
    }

    public void CheckForPlayer(EnemyStateMachine enemy)
    {
        // Implement the logic for checking if the player is within the search radius
        // For example, you can use a sphere cast or distance check to see if the player is nearby
        if ((enemy.GetComponent<ZonesOfHearing>().playerNear || enemy.GetComponent<ZonesOfHearing>().playerWalk || enemy.GetComponent<ZonesOfHearing>().playerSprint)
            && !enemy.GetComponent<FieldOfView>().canSeePlayer)
        {
            Debug.Log("Player found!");
            currentState = searchState;
            currentState.EnterState(this);
        }
        
    }
}
