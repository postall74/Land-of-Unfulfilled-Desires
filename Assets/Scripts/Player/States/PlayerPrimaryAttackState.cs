using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;
    private float _lastTimeAttacked;

    private float ChoosingDirectionOfAttack()
    {
        //xInput = 0; // if we have a bug with direction attack, we reset xInput to 0...
        float attackDir = player.FacingDirection;

        if (xInput != 0)
            attackDir = xInput;

        return attackDir;
    }

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + player.ComboAttackWaitingTime)
            _comboCounter = 0;

        player.Animator.SetInteger("ComboCounter", _comboCounter);
        player.SetVelocity(player.AttackMovement[_comboCounter].x * ChoosingDirectionOfAttack(), player.AttackMovement[_comboCounter].y);
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(player.ComboAttackWaitingTime));
        _comboCounter++;
        _lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
