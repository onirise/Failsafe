using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private ZonesOfHearing _zonesOfHearing;

    public override void EnterState(EnemyStateMachine enemy)
    {
        InicializeComponents(enemy);
        _navMeshAgent.SetDestination(enemy.transform.position);
        StartAttack(enemy);
        Debug.Log("Entering Attack State");
    }

    public override void ExitState(EnemyStateMachine enemy)
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = enemy.normalSpeed;
        }

        if (enemy.enemyWeapon != null)
        {
            enemy.enemyWeapon.GetComponent<Collider>().isTrigger = false;
        }

        if (_animator != null)
        {
            _animator.ResetTrigger("isAttacking");
        }

        Debug.Log("Exiting Attack State");
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (_zonesOfHearing == null || _navMeshAgent == null) return;

        StopNear(enemy);
        TurnToPlayer(enemy);

        if (!_zonesOfHearing.playerNear)
        {
            enemy.SwitchState<EnemyPatrolingState>();
        }
    }

    private void StopNear(EnemyStateMachine enemy)
    {
        if (_zonesOfHearing.playerNear)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.speed = 0;
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = enemy.normalSpeed;
        }
    }

    private void TurnToPlayer(EnemyStateMachine enemy)
    {
        if (_zonesOfHearing.playerNear)
        {
            Vector3 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void InicializeComponents(EnemyStateMachine enemy)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        _animator = enemy.GetComponent<Animator>();
        _zonesOfHearing = enemy.GetComponent<ZonesOfHearing>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("Не удалось найти необходимые компоненты на объекте врага _navMeshAgent.");
            return;
        }
        if (_animator == null) 
        {
            Debug.LogError("Не удалось найти необходимые компоненты на объекте врага _animator.");
            return;
        }

         if (_zonesOfHearing == null)
        {
            Debug.LogError("Не удалось найти необходимые компоненты на объекте врага _zonesOfHearing.");
            return;
        }
            
        
    }

    private void StartAttack(EnemyStateMachine enemy)
    {
        if (_animator != null)
        {
            enemy.enemyWeapon.GetComponent<Collider>().isTrigger = true;
            _animator.SetTrigger("isAttacking");
        }
    }
}


