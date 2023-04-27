using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;

    private string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this._player = player;
        this._stateMachine = stateMachine;
        this._animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        _player.Animator.SetBool(_animBoolName, true);
    }

    public virtual void Update() 
    {
    }

    public virtual void Exit()
    {
        _player.Animator.SetBool(_animBoolName, false);
    }
}
