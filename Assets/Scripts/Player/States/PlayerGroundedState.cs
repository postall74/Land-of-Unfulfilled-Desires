using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.AimSwordState);

        if (Input.GetKeyDown(KeyCode.Q))
            stateMachine.ChangeState(player.CounterAttackState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.PrimaryAttackState);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.AirState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.JumpState);
    }

    private bool HasNoSword()
    {
        if (!player.Sword)
            return true;

        player.Sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
