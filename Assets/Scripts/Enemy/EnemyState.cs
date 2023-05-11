using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected float stateTimer;
    protected bool triggerCalled;

    private string _animBoolName;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine,  string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.Animator.SetBool(_animBoolName, true);
    }

    public virtual void Update() 
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit() 
    {
        enemyBase.Animator.SetBool(_animBoolName, false);
    }
}
