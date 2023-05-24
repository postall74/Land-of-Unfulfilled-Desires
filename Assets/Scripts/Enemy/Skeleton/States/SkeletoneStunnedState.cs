using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletoneStunnedState : EnemyState
{
    private Enemy_skeleton _enemy;

    public SkeletoneStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_skeleton enemy) : base(enemy, stateMachine, animBoolName)
    {
        _enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.EntityFX.InvokeRepeating("RedColorBlink", 0f, .1f);
        stateTimer = _enemy.StunDuration;
        _enemy.Rigidbody.velocity = new Vector2(-_enemy.FacingDirection * _enemy.StunDirection.x, _enemy.StunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        _enemy.EntityFX.Invoke("CancelRedBlink", 0f);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(_enemy.IdleState);
    }
}
