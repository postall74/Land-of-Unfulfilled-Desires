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

        player.Skill.Clone.CreateCloneOnDashStart();
        stateTimer = player.DashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.Skill.Clone.CreateCloneOnDashOver();
        player.SetVelocity(0, rigidbody.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundDetected() && player.IsWallDetected() ) 
            stateMachine.ChangeState(player.WallSlideState);

        player.SetVelocity(player.DashSpeed * player.DashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.IdleState);
    }
}
