using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_skeleton enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.IdleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}
