using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_skeleton _enemy => GetComponentInParent<Enemy_skeleton>();

    private void AnimationTrigger()
    {
        _enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemy.AttackCheck.position, _enemy.AttackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damage();
        }
    }

    private void OpenCounterAttackWindow() => _enemy.SetCounterAttackWindow(true);

    private void CloseCounterAttackWindow() => _enemy.SetCounterAttackWindow(false);
}
