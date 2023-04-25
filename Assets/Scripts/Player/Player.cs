using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Attack info")]
    [SerializeField] private float _comboTime = .3f;
    private float _comboTimeWindow;
    private bool _isAttacking;
    private int _comboCounter;
    [Header("Movment info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [Header("Dash info")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;
    private float _dashTime;
    private float _dashCooldownTimer;
    [Header("Collision info")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _ground;

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _xInput;
    private bool _faceRight = true;
    private float _faceDirection = 1;
    private bool _isGround;

    public void AttackOver()
    {
        _isAttacking = false;
        _comboCounter++;

        if (_comboCounter > 2)
            _comboCounter = 0;

    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Movement();
        CheckInput();
        CollisionChecks();

        _dashTime -= Time.deltaTime;
        _dashCooldownTimer -= Time.deltaTime;
        _comboTimeWindow -= Time.deltaTime;

        FlipController();
        AnimatorController();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - _groundCheckDistance));
    }

    private void CheckInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            DashAbility();
    }

    private void StartAttackEvent()
    {
        if (!_isGround)
            return;

        if (_comboTimeWindow < 0)
            _comboCounter = 0;

        _isAttacking = true;
        _comboTimeWindow = _comboTime;
    }

    private void DashAbility()
    {
        if (_dashCooldownTimer < 0 && !_isAttacking)
        {
            _dashCooldownTimer = _dashCooldown;
            _dashTime = _dashDuration;
        }
    }

    private void Movement()
    {
        if (_isAttacking)
            _rb.velocity = new Vector2(0, 0);
        else if (_dashTime > 0)
            _rb.velocity = new Vector2(_faceDirection * _dashSpeed, 0);
        else
            _rb.velocity = new Vector2(_xInput * _moveSpeed, _rb.velocity.y);
    }

    private void Jump()
    {
        if (_isGround)
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
    }

    private void CollisionChecks()
    {
        _isGround = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _ground);
    }

    private void AnimatorController()
    {
        bool isMoving = _rb.velocity.x != 0;
        _animator.SetFloat("yVelocity", _rb.velocity.y);
        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("isGrounded", _isGround);
        _animator.SetBool("isDashing", _dashTime > 0);
        _animator.SetBool("isAttacking", _isAttacking);
        _animator.SetInteger("comboCounter", _comboCounter);
    }

    private void Flip()
    {
        _faceDirection *= -1;
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if ((_rb.velocity.x > 0 && !_faceRight) || (_rb.velocity.x < 0 && _faceRight)) Flip();
    }
}
