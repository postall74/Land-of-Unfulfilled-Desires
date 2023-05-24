using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Tilemaps;
using UnityEngine;


public class Player : Entity
{
    #region Fields
    [Header("Move info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _wallJumpForce;
    [SerializeField] private float _slidCoefficient;

    [Header("Attack details")]
    [SerializeField] private Vector2[] _attackMovement;
    [SerializeField] private float _comboAttackWaitingTime;
    [SerializeField] private float _counterAttackDuration = .2f;

    [Header("Dash info")]
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;

    private float _dashDirection;
    private float _dashUsageTimer;
    #endregion

    #region Properties
    public float MoveSpeed => _moveSpeed;
    public float JumpForce => _jumpForce;
    public Vector2[] AttackMovement => _attackMovement;
    public float ComboAttackWaitingTime => _comboAttackWaitingTime;
    public float CounterAttackDuration => _counterAttackDuration;
    public float WallJumpForce => _wallJumpForce;
    public float SlidCoefficient => _slidCoefficient;
    public float DashDuration => _dashDuration;
    public float DashSpeed => _dashSpeed;
    public float DashDir => _dashDirection;
    public bool IsBusy { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    #endregion

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public IEnumerator BusyFor(float _seconds)
    {
        IsBusy = true;
        yield return new WaitForSeconds(_seconds);
        IsBusy = false;
    }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
        CounterAttackState = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
        CheckForDashInput();
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        _dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashUsageTimer < 0)
        {
            _dashUsageTimer = _dashCooldown;
            _dashDirection = Input.GetAxisRaw("Horizontal");

            if (_dashDirection == 0)
                _dashDirection = FacingDirection;

            StateMachine.ChangeState(DashState);
        }
    }
}
