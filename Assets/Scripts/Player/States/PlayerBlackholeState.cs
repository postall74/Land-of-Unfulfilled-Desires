using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    #region Fields
    private float _flyTime = .4f;
    private bool _skillUsed;
    private float _defaultGravity;
    #endregion

    public PlayerBlackholeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        _skillUsed = false;
        stateTimer = _flyTime;
        _defaultGravity = player.Rigidbody.gravityScale;
        rigidbody.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.Rigidbody.gravityScale = _defaultGravity;
        player.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
            rigidbody.velocity = new Vector2(0, 15);

        if (stateTimer < 0)
        {
            rigidbody.velocity = new Vector2(0, -.1f);

            if (!_skillUsed)
            {
                if(player.Skill.Blackhole.CanUseSkill())
                    _skillUsed = true;
            }
        }

        if (player.Skill.Blackhole.SkillCompleted())
            stateMachine.ChangeState(player.AirState);
    }
}
