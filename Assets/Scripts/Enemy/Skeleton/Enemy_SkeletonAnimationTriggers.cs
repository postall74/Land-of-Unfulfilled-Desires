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
}
