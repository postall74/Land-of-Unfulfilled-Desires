using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        { 
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }

        if (xInput != 0 && player.FacingDirection != xInput)
            stateMachine.ChangeState(player.IdleState);

        if (yInput < 0)
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        else
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y * player.SlidCoefficient);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.IdleState);
    }
}
