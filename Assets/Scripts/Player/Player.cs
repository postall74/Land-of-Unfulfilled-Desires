using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _xInput;
    private int _faceDirection = 1;
    private bool _faceRight = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Movement();
        CheckInput();
        FlipController();
        AnimatorController();
    }

    private void CheckInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void Movement()
    {
        _rb.velocity = new Vector2(_xInput * _moveSpeed, _rb.velocity.y);
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
    }

    private void AnimatorController()
    {
        bool _isMoving = _rb.velocity.x != 0;
        _animator.SetBool("isMoving", _isMoving);
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
