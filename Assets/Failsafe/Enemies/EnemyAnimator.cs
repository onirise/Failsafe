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

    private void HandleRotation()
    {
        if (_isTurning)
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
            // → Поворот на 180°
            if (Mathf.Abs(angle) >= _turn180Min)
            {
                _animator.SetTrigger("Turn180");
                _controller.StopMoving();
                return;
            }

            // → Поворот направо на 90°
            if (angle >= _turn90Min && angle < _turn180Min)
            {
                _animator.SetTrigger("TurnRight90");
                _controller.StopMoving();
                return;
            }

            // → Поворот налево на 90°
            if (angle <= -_turn90Min && angle > -_turn180Min)
            {
                _animator.SetTrigger("TurnLeft90");
                _controller.StopMoving();
                return;
            }
        }

        // Плавный поворот при движении
        _transform.forward = Vector3.Slerp(
            _transform.forward,
            direction,
            Time.deltaTime * 5f
        );
    }

    private void SeesPlayerAnimation()
    {

    }

    public void ApplyRootMotion()
    {
        _transform.position = _animator.rootPosition;
        _transform.rotation = _animator.rootRotation;
        _navMeshAgent.nextPosition = _transform.position;
    }
}
