using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CircleCollider2D))]
public class CrystalSkillController : MonoBehaviour
{
    #region Components
    private Animator _anim;
    private CircleCollider2D _collider;
    #endregion

    #region Fields
    private float _crystalExistTimer;
    private bool _canExplode;
    private bool _canMove;
    private float _moveSpeed;
    private bool _canGrow;
    private float _growSpeed = 5;
    private Transform _closestTarget;
    #endregion

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed, Transform closestTarget)
    {
        _crystalExistTimer = crystalDuration;
        _canExplode = canExplode;
        _canMove = canMove;
        _moveSpeed = moveSpeed;
        _closestTarget = closestTarget;
    }

    public void FinishCrystal()
    {
        if (_canExplode)
        {
            _canGrow = true;
            _anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        _crystalExistTimer -= Time.deltaTime;

        if (_crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (_canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _closestTarget.position, _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _closestTarget.position) < 1)
            {
                FinishCrystal();
                _canMove = false;
            }
        }

        if (_canGrow)
            transform.localScale = transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), _growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _collider.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}
