using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.DashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rigidbody.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.DashSpeed * player.FacingDirection, rigidbody.velocity.y);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.IdleState);
    }
}
