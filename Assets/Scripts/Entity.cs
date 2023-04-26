using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision info")]
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected LayerMask _ground;
    [Space]
    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected float _wallCheckDistance;

    /// <summary>
    /// Сыылки на основные компаненты на объектах Rigidbody & Animator
    /// </summary>
    protected Animator _animator;
    protected Rigidbody2D _rb;
    /// <summary>
    /// Переменные для метода определения столкновения CollisionChecks()
    /// </summary>
    protected bool _isGround;
    protected bool _isWallDetected;
    /// <summary>
    /// Переменные для метода Flip()
    /// </summary>
    protected bool _faceRight = true;
    protected float _faceDirection = 1;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        if (_wallCheck == null)
            _wallCheck = transform;
    }

    protected virtual void Update()
    {
        CollisionChecks();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector3(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x + _wallCheckDistance * _faceDirection, _wallCheck.position.y));
    }

    protected virtual void CollisionChecks()
    {
        _isGround = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _ground);
        _isWallDetected = Physics2D.Raycast(_wallCheck.position, Vector2.right, _wallCheckDistance * _faceDirection, _ground);
    }

    protected virtual void Flip()
    {
        _faceDirection *= -1;
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }
}
