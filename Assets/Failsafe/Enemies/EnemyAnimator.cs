using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator
{
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly Transform _transform;
    private EnemyAudioManager _audioManager;
    private bool _isTurning = false;

    private bool _waitingForTurnToFinish = false;
    private bool _inCooldown = false;
    private bool _inAttack = false;

    public EnemyAnimator( NavMeshAgent navMeshAgent, Animator animator, Transform transform)
    {
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _transform = transform;
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    public void UpdateAnimator()
    {
        if (IsInAction())
            return;

        if (_isTurning)
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);
    
            if (state.IsTag("Turn") && state.normalizedTime >= 0.98f)
            {
                _isTurning = false;
                _animator.SetFloat("TurnAngle", 0f);
            }

            _animator.SetFloat("Speed", 0f);
            return;
        }

        // Вход в поворот
        if (ShouldStartTurn(out float clampedAngle))
        {
            _isTurning = true;
            _animator.SetFloat("TurnAngle", clampedAngle);
            _animator.CrossFade("TurnInPlace", 0.1f);
            _animator.SetFloat("Speed", 0f);
            return;
        }

        UpdateSpeedBlend();
    }
    private bool ShouldStartTurn(out float clampedAngle)
    {
        clampedAngle = 0f;

        Vector3 desiredDirection = _navMeshAgent.desiredVelocity;
        if (desiredDirection.sqrMagnitude < 0.01f)
            return false;

        if (_navMeshAgent.velocity.magnitude > 0.1f)
            return false;

        float signedAngle = Vector3.SignedAngle(_transform.forward, desiredDirection.normalized, Vector3.up);

        if (Mathf.Abs(signedAngle) < 25f)
            return false; 

        clampedAngle = Mathf.Clamp(signedAngle, -180f, 180f);
        return true;
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
        // Получаем текущую позицию агента на навмеш
        Vector3 agentNextPos = _navMeshAgent.nextPosition;

        // Считаем дельту из root motion
        Vector3 rootDelta = _animator.deltaPosition;
        rootDelta.y = 0f;

        // Предлагаемую новую позицию
        Vector3 proposedPos = _transform.position + rootDelta;

        // Обновляем позицию агента
        _navMeshAgent.nextPosition = proposedPos;

        // Перемещаем трансформ только в пределах навмеша
        _transform.position = _navMeshAgent.nextPosition;

        // Поворот
        if (_isTurning)
        {
            _transform.rotation = _animator.rootRotation;
        }
        else
        {
            Vector3 desiredVelocity = _navMeshAgent.desiredVelocity;
            if (desiredVelocity.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(desiredVelocity.normalized);
                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    public void TryAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public bool IsInAction()
    {
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        return (state.IsTag("Attack") || state.IsTag("Reload")) && _inAttack;
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

    public void isAttacking(bool isAttacking)
    {
        _inAttack = isAttacking;
    }

    public void StartMove(float speed)
    {
        _animator.SetFloat("Speed", speed );
    }

}
