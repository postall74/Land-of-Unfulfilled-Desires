using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity
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

    private float _xInput;

    public void AttackOver()
    {
        _isAttacking = false;
        _comboCounter++;

        if (_comboCounter > 2)
            _comboCounter = 0;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Movement();
        CheckInput();
        
        _dashTime -= Time.deltaTime;
        _dashCooldownTimer -= Time.deltaTime;
        _comboTimeWindow -= Time.deltaTime;

        FlipController();
        AnimatorController();
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

    private void FlipController()
    {
        if ((_rb.velocity.x > 0 && !_faceRight) || (_rb.velocity.x < 0 && _faceRight)) Flip();
    }
}
