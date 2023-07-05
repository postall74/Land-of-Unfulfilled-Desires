using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CloneSkillController : MonoBehaviour
{
    #region Components
    private SpriteRenderer _sr;
    private Animator _anim;
    #endregion

    #region Fields
    [SerializeField] private float _colorLoosingSpeed;
    [SerializeField] private Transform _attackCheck;
    [SerializeField] private float _attackCheckRadius = .8f;
    private float _cloneTimer;
    private Transform _closestEnemy;
    private int _faceDirection = 1;

    private float _chanceToDuplicate;
    private bool _canDuplicateClone;
    #endregion

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform closestEnemy, bool canDuplicateClone, float chanceToDuplicate)
    {
        if (canAttack)
            _anim.SetInteger("AttackNumber", Random.Range(1, 3));

        transform.position = newTransform.position + offset;
        _cloneTimer = cloneDuration;
        _closestEnemy = closestEnemy;
        _canDuplicateClone = canDuplicateClone;
        _chanceToDuplicate = chanceToDuplicate;
        FaceClosestTarget();
    }

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _cloneTimer -= Time.deltaTime;

        if (_cloneTimer < 0)
        {
            _sr.color = new Color(1, 1, 1, _sr.color.a - (Time.deltaTime * _colorLoosingSpeed));

            if (_sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    private void AnimationTrigger()
    {
        _cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheck.position, _attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();

                if (_canDuplicateClone)
                {
                    if (Random.Range(0,100) < _chanceToDuplicate)
                    {
                        SkillManager.instance.Clone.CreateClone(hit.transform, new Vector3(.5f * _faceDirection, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (_closestEnemy != null)
        {
            if (transform.position.x > _closestEnemy.position.x)
            {
                _faceDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
