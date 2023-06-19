using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    #region Fields
    [Header("Move info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _idleTime;
    [SerializeField] private float _battleTime;
    private float _defaultMoveSpeed;
    [Space]
    [Header("Player Detected")]
    [SerializeField] private LayerMask _whatIsPlayer;
    [Space]
    [Header("Attack info")]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _attackCooldown;
    [HideInInspector] private float _lastTimeAttacked;
    [Header("Stunned info")]
    [SerializeField] private float _stunDuration;
    [SerializeField] private Vector2 _stunDirection;
    [SerializeField] protected GameObject _counterImage;

    protected bool _canBeStunned;
    #endregion

    #region Properties
    public float MoveSpeed => _moveSpeed;
    public float IdleTime => _idleTime;
    public float BattleTime => _battleTime;
    public float AttackDistance => _attackDistance;
    public float AttackCooldown => _attackCooldown;
    public float LastTimeAttacked => _lastTimeAttacked;
    public float StunDuration => _stunDuration;
    public Vector2 StunDirection => _stunDirection;
    #endregion

    #region States
    public EnemyStateMachine StateMachine { get; private set; }
    #endregion

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, 50, _whatIsPlayer);

    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public virtual void SetLastTimeAttack() => _lastTimeAttacked = Time.time;

    public virtual void SetCounterAttackWindow(bool state)
    {
        _canBeStunned = state;
        _counterImage.SetActive(state);
    }

    public virtual bool CanBeStunned()
    {
        if (_canBeStunned)
        {
            SetCounterAttackWindow(false);
            return true;
        }

        return false;
    }

    public virtual void FreezeTime(bool isTimeFrozen)
    {
        if (isTimeFrozen)
        {
            _moveSpeed = 0;
            Animator.speed = 0;
        }
        else
        {
            _moveSpeed = _defaultMoveSpeed;
            Animator.speed = 1;
        }
    }

    public virtual IEnumerator FreezeTimerFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
        _defaultMoveSpeed = _moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + _attackDistance * FacingDirection, transform.position.y));
    }

}
