using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _xInput;
    private bool _isMoving;

    void Start()
    {
        _rb= GetComponent<Rigidbody2D>();
        _animator= GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(_xInput * _moveSpeed, _rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

        _isMoving = _rb.velocity.x != 0;
        _animator.SetBool("isMoving", _isMoving);
    }
}
