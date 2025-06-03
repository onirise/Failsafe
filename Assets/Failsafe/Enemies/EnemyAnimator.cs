using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator
{
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly Transform _transform;
    private EnemyController _controller;

    private bool _isTurning = false;
    private const float _turn90Min = 60f;
    private const float _turn90Max = 135f;
    private const float _turn180Min = 135f;

    private bool _waitingForTurnToFinish = false;
    private bool _inCooldown = false;
    public bool UseRootRotation = true; // флаг управления поворотом

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
       
        UpdateTurningState();

        if (IsInAction())
            return; // Блокируем всё, пока идёт атака/перезарядка

        HandleRotation();
        SeesPlayerAnimation();
        UpdateSpeedBlend();
    }
    public void ApplyRoot()
    {
        _animator.applyRootMotion = true;

    }
    private void UpdateTurningState()
    {
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        _isTurning = state.IsTag("Turning");

        if (_waitingForTurnToFinish && !_isTurning)
        {
            _waitingForTurnToFinish = false;
            _controller.ResumeMoving();
        }
    }

    private void UpdateSpeedBlend()
    {
        if (_isTurning)
        {
            _animator.SetFloat("Speed", 0f);
            return;
        }

        float velocity = _navMeshAgent.velocity.magnitude;
        _animator.SetFloat("Speed", velocity, 0.15f, Time.deltaTime);
    }
    public void SetUseRootRotation(bool enabled)
    {
        UseRootRotation = enabled;
        _animator.applyRootMotion = enabled;
    }

    private bool _turningAnimationTriggered = false;

    private void HandleRotation()
    {
        if (_isTurning || _turningAnimationTriggered)
        {
            _waitingForTurnToFinish = true;
            return;
        }

        Vector3 direction = (_navMeshAgent.steeringTarget - _transform.position).normalized;
        if (direction.sqrMagnitude < 0.01f)
            return;

        float angle = Vector3.SignedAngle(_transform.forward, direction, Vector3.up);

        if (_navMeshAgent.velocity.magnitude < 0.1f)
        {
            if (Mathf.Abs(angle) >= _turn180Min)
            {
                _animator.SetTrigger("Turn180");
                _controller.StopMoving();
                _turningAnimationTriggered = true;
                return;
            }

            if (angle >= _turn90Min && angle < _turn180Min)
            {
                _animator.SetTrigger("TurnRight90");
                _controller.StopMoving();
                _turningAnimationTriggered = true;
                return;
            }

            if (angle <= -_turn90Min && angle > -_turn180Min)
            {
                _animator.SetTrigger("TurnLeft90");
                _controller.StopMoving();
                _turningAnimationTriggered = true;
                return;
            }
        }

        _transform.forward = Vector3.Slerp(_transform.forward, direction, Time.deltaTime * 5f);
    }

    private void SeesPlayerAnimation()
    {

    }

    public void ApplyRootMotion()
    {
        Vector3 rootPos = _animator.rootPosition;
        rootPos.y = _navMeshAgent.nextPosition.y;
        _transform.position = rootPos;

        if (UseRootRotation)
        {
            _transform.rotation = _animator.rootRotation;
        }

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
