using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    private Enemy_skeleton _enemy;

    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine stateMachine,  string animBoolName, Enemy_skeleton enemy) : base(enemy, stateMachine, animBoolName)
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
    }

    public override void Update()
    {
        base.Update();
        _enemy.SetVelocity(_enemy.MoveSpeed * _enemy.FacingDirection, enemyBase.Rigidbody.velocity.y);

        if (_enemy.IsWallDetected() || !_enemy.IsGroundDetected())
        {
            _enemy.Flip();
            stateMachine.ChangeState(_enemy.IdleState);
        }
    }
}
