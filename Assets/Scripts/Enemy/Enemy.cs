using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    #region Fields
    [Header("Move info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _idleTime;
    [Space]
    [Header("Player Detected")]
    [SerializeField] private LayerMask _whatIsPlayer;
    [Space]
    [Header("Attack info")]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _attackCooldown;
    [HideInInspector] private float _lastTimeAttacked;
    #endregion

    #region Properties
    public float MoveSpeed => _moveSpeed;
    public float idleTime => _idleTime;
    //public LayerMask WhatIsPlayer => _whatIsPlayer;
    public float AttackDistance => _attackDistance;
    public float AttackCooldown => _attackCooldown;
    public float LastTimeAttacked => _lastTimeAttacked;
    #endregion

    #region States
    public EnemyStateMachine StateMachine { get; private set; }
    #endregion

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, 50, _whatIsPlayer);

    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public virtual void SetLastTimeAttack() => _lastTimeAttacked = Time.time;

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
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
