using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _xInput;
    private bool _faceRight = true;
    private bool _isGround;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [Header("Attack info")]
    [SerializeField] private bool _isAttack;
    [SerializeField] private int _comboCounter;
    [Header("Dash info")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashTime;
    [Header("Collision info")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _ground;

    public void AttackOver() =>
        _isAttack = false;

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _dashTime = _dashDuration;
 
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
            _isAttack = true;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void Movement()
    {
        if (_dashTime > 0)
            _rb.velocity = new Vector2(_xInput * _dashSpeed, 0);
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
        _animator.SetBool("isAttacking", _isAttack);
        _animator.SetInteger("comboCounter", _comboCounter);
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if ((_rb.velocity.x > 0 && !_faceRight) || (_rb.velocity.x < 0 && _faceRight)) Flip();
    }
}
