using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class SwordSkillController : MonoBehaviour
{
    #region Fields
    [Header("Regular info")]
    private float _freezeTimeDuration;
    [Header("Bounce info")]
    private float _bounceSpeed;
    private List<Transform> _enemyTarget;
    private bool _isBouncing;
    private int _bounceAmount;
    private int _targetIndex;
    [Header("Pierce info")]
    private float _pierceAmount;
    [Header("Spin info")]
    private float _maxTravelDistance;
    private float _spinDuration;
    private float _spinTimer;
    private bool _wasStoped;
    private bool _isSpinning;
    //private float _spinDirection;
    [Header("Skill info")]
    private float _returnSpeed = 12f;
    private bool _canRotate = true;
    private bool _isReturning = false;
    private float _hitTimer;
    private float _hitCooldown;
    #endregion

    #region Components
    private Animator _anim;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;
    private Player _player;
    #endregion

    #region Properties
    public bool IsBouncing => _isBouncing;
    public int AmountOfBounce => _bounceAmount;
    public float BounceSpeed => _bounceSpeed;
    public List<Transform> EnemyTarget => _enemyTarget;
    #endregion

    public void SetupSword(Vector2 direction, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)
    {
        _player = player;
        _rb.velocity = direction;
        _rb.gravityScale = gravityScale;
        _freezeTimeDuration = freezeTimeDuration;

        if (_pierceAmount <= 0)
            _anim.SetBool("Rotation", true);

        Invoke(nameof(DestroySword), 7f);

        //_spinDirection = Mathf.Clamp(_rb.velocity.x, -1, 1);
    }

    private void DestroySword() => Destroy(gameObject);

    public void SetupBounce(bool isBouncing, int amountOfBounce, float bounceSpeed)
    {
        _isBouncing = isBouncing;
        _bounceAmount = amountOfBounce;
        _bounceSpeed = bounceSpeed;
        _enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int pierceAmount) => _pierceAmount = pierceAmount;

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        _isSpinning = isSpinning;
        _maxTravelDistance = maxTravelDistance;
        _spinDuration = spinDuration;
        _hitCooldown = hitCooldown;
    }

    public void ReturnSword()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        _isReturning = true;
    }

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (_canRotate)
            transform.right = _rb.velocity;

        if (_isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _player.transform.position) < 1)
                _player.CatchTheSword();
        }

        BounceLogic();

        SpinLogic();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine(enemy.FreezeTimerFor(_freezeTimeDuration));
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (_isBouncing && _enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        _enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (_pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            _pierceAmount--;
            return;
        }

        if (_isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        _canRotate = false;
        _collider.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_isBouncing && _enemyTarget.Count > 0)
            return;

        _anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    private void BounceLogic()
    {
        if (_isBouncing && _enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTarget[_targetIndex].position, _bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _enemyTarget[_targetIndex].position) < .1f)
            {
                SwordSkillDamage(_enemyTarget[_targetIndex].GetComponent<Enemy>());
                _targetIndex++;
                _bounceAmount--;

                if (_bounceAmount <= 0)
                {
                    _isBouncing = false;
                    _isReturning = true;
                }

                if (_targetIndex >= _enemyTarget.Count)
                    _targetIndex = 0;
            }
        }
    }

    private void SpinLogic()
    {
        if (_isSpinning)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_wasStoped)
                StopWhenSpinning();

            if (_wasStoped)
            {
                _spinTimer -= Time.deltaTime;
                //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                //

                if (_spinTimer < 0)
                {
                    _isReturning = true;
                    _isSpinning = false;
                }

                _hitTimer -= Time.deltaTime;

                if (_hitTimer < 0)
                {
                    _hitTimer = _hitCooldown;
                    HitDamage(1f);
                }
            }
        }
    }

    private void HitDamage(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                SwordSkillDamage(hit.GetComponent<Enemy>());
            }
        }
    }

    private void StopWhenSpinning()
    {
        _wasStoped = true;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _spinTimer = _spinDuration;
    }
}
