using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform _player;
    private Enemy_skeleton _enemy;
    private int _moveDir;

    public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_skeleton enemy) : base(enemy, stateMachine, animBoolName)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        _player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.IsPlayerDetected())
        {
            stateTimer = _enemy.BattleTime;

            if (_enemy.IsPlayerDetected().distance < _enemy.AttackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(_enemy.AttackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(_player.transform.position, _enemy.transform.position) < 15)
                stateMachine.ChangeState(_enemy.IdleState);
        }

        if (_player.position.x > _enemy.transform.position.x)
            _moveDir = 1;
        else if (_player.position.x < _enemy.transform.position.x)
            _moveDir = -1;

        _enemy.SetVelocity(_enemy.MoveSpeed * _moveDir, _enemy.Rigidbody.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time >= _enemy.LastTimeAttacked + _enemy.AttackCooldown)
        {
            _enemy.SetLastTimeAttack();
            return true;
        }

        return false;
    }
}
