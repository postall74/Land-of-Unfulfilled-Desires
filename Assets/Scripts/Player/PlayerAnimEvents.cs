using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimEvents : MonoBehaviour
{

    private Player _player;

    private void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    private void AnimationTrigger()
    {
        _player.AttackOver();
    }
}
