using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rigidbody;
    protected float xInput;
    protected float yInput;
    protected float stateTimer;

    private string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this._animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.Animator.SetBool(_animBoolName, true);
        rigidbody = player.Rigidbody;
    }

    public virtual void Update() 
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.Animator.SetFloat("yVelocity", rigidbody.velocity.y);
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(_animBoolName, false);
    }
}
