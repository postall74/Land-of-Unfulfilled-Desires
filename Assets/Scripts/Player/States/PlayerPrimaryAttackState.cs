using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;
    private float _lastTimeAttacked;
    
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + player.ComboAttackWaitingTime)
            _comboCounter = 0;

        player.Animator.SetInteger("ComboCounter", _comboCounter);
    }

    public override void Exit()
    {
        base.Exit();

        _comboCounter++;
        _lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
