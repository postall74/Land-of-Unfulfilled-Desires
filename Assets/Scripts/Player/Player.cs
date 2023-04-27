using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Move info")]
    [SerializeField] private float _moveSpeed;

    #region Properties
    public float MoveSpeed => _moveSpeed;
    #endregion

    #region Component
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    #endregion

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        Rigidbody.velocity = new Vector2(_xVelocity, _yVelocity);
    }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
    }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }
}
