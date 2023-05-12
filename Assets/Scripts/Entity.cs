using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    #region Fields
    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [Space]
    [SerializeField] protected LayerMask groundLayer;

    protected bool _isFacingRight = true;
    #endregion

    #region Properties
    public int FacingDirection { get; private set; } = 1;
    #endregion

    #region Component
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    #endregion

    #region Collision Methods
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, wallCheckDistance, groundLayer);
    #endregion

    #region Velocity Methods
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        Rigidbody.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void SetZeroVelocity() => Rigidbody.velocity = new Vector2(0, 0);
    #endregion

    #region Flip Methods
    public virtual void Flip()
    {
        FacingDirection *= -1;
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !_isFacingRight)
            Flip();
        else if (x < 0 && _isFacingRight)
            Flip();
    }
    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
