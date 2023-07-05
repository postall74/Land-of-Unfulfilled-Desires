using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    #region Fields
    private bool _canCreateClone;
    #endregion

    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        _canCreateClone = true;
        stateTimer = player.CounterAttackDuration;
        player.Animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && hit.GetComponent<Enemy>().CanBeStunned())
            {
                stateTimer = 10;
                player.Animator.SetBool("SuccessfulCounterAttack", true);

                if (_canCreateClone)
                {
                    _canCreateClone = false;
                    player.Skill.Clone.CreateCloneOnCounterAttack(hit.transform);
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
