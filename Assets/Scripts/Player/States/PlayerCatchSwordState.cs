using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    #region Fields
    private Transform _sword;
    #endregion

    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        _sword = player.Sword.transform;

        if (player.transform.position.x > _sword.position.x && player.FacingDirection == 1)
            player.Flip();
        else if (player.transform.position.x < _sword.position.x && player.FacingDirection == -1)
            player.Flip();

        rigidbody.velocity = new Vector2(player.SwordReturnImpact * -player.FacingDirection, rigidbody.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(.1f));
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
