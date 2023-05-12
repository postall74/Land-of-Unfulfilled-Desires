using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Enemy_skeleton _enemy;
    public SkeletonAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_skeleton enemy) : base(enemy, stateMachine, animBoolName)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        _enemy.SetLastTimeAttack();
    }

    public override void Update()
    {
        base.Update();

        _enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(_enemy.BattleState);
    }
}
