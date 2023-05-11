using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    #region Fields
    [Header("Move info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _idleTime;
    #endregion

    #region Properties
    public float MoveSpeed => _moveSpeed;
    public float idleTime => _idleTime;
    #endregion

    #region States
    public EnemyStateMachine StateMachine { get; private set; }
    #endregion

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
}
