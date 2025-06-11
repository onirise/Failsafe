using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator
{
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly Transform _transform;
    private EnemyController _controller;
    private EnemyAudioManager _audioManager;
    private bool _isTurning = false;

    private bool _waitingForTurnToFinish = false;
    private bool _inCooldown = false;

    public EnemyAnimator( NavMeshAgent navMeshAgent, Animator animator, Transform transform, EnemyController enemyController)
    {
        _controller = enemyController;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _transform = transform;
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    public void UpdateAnimator()
    {
       
        if (IsInAction())
            return; // Блокируем всё, пока идёт атака/перезарядка
        UpdateSpeedBlend();
    }
    public void ApplyRoot()
    {
        _animator.applyRootMotion = true;

    }
   

    private void UpdateSpeedBlend()
    {
        if (_isTurning)
        {
            _animator.SetFloat("Speed", 0f);
            return;
        }

        float velocity = _navMeshAgent.velocity.magnitude;
        
        _animator.SetFloat("Speed", velocity);
    }


    public void ApplyRootMotion()
    {
        // Перемещение из анимации
        Vector3 rootPos = _animator.rootPosition;
        rootPos.y = _navMeshAgent.nextPosition.y;
        _transform.position = rootPos;

        // Поворот к направлению движения
        Vector3 desiredVelocity = _navMeshAgent.desiredVelocity;

        if (desiredVelocity.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredVelocity.normalized);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 10f); // smooth поворот
        }

        // Синхронизируем Agent с RootMotion-позицией
        _navMeshAgent.nextPosition = _transform.position;
    }

    public void TryAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public bool IsInAction()
    {
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        return state.IsTag("Attack") || state.IsTag("Reload");
    }

    public void TryReload()
    {
        _animator.SetTrigger("Reload");
    }

    public void isReloading(bool isReloading)
    {
        _inCooldown = isReloading;
        _animator.SetBool("isReloading", isReloading);
    }
    
    
}
