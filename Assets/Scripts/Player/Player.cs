using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Move info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _slidCoefficient;

    [Header("Dash info")]
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;

    [Header("Collision info")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckDistance;
    [Space]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckDistance;
    [Space]
    [SerializeField] private LayerMask _groundLayer;

    private bool _isFacingRight = true;
    private float _dashDirection;
    private float _dashUsageTimer;

    #region Properties
    public float MoveSpeed => _moveSpeed;
    public float JumpForce => _jumpForce;
    public float SlidCoefficient => _slidCoefficient;
    public float DashDuration => _dashDuration;
    public float DashSpeed => _dashSpeed;
    public float DashDir => _dashDirection;
    public int FacingDirection { get; private set; } = 1;
    #endregion

    #region Component
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    #endregion

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        Rigidbody.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundLayer);

    public bool IsWallDetected() => Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDirection, _wallCheckDistance, _groundLayer);

    public void Flip()
    {
        FacingDirection *= -1;
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float x)
    {
        if (x > 0 && !_isFacingRight)
            Flip();
        else if (x < 0 && _isFacingRight)
            Flip();
    }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
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
        CheckForDashInput();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector3(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));
    }

    private void CheckForDashInput()
    {
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
