using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_sceletone : Entity
{
    [Header("Movment info")]
    [SerializeField] private float _moveSpeed;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!_isGround || _isWallDetected)
            Flip();

        _rb.velocity = new Vector2(_moveSpeed * _faceDirection, _rb.velocity.y);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
