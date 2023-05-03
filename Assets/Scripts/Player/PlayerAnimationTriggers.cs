using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player _player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        _player.AnimationTrigger();
    }
}
