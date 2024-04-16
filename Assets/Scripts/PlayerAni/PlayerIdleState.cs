using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private PlayerFSM  P_fsm;
    private PlayerParameter P_parameter;

    public PlayerIdleState(PlayerFSM fsm) {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;

    }
    public void OnEnter()
    {
        P_parameter.Player_Animator.Play("Idle");
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}

public class PlayerWalkState : IPlayerState
{
    private PlayerFSM P_fsm;
    private PlayerParameter P_parameter;

    public PlayerWalkState(PlayerFSM fsm)
    {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;

    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}
public class PlayerJumpState : IPlayerState
{
    private PlayerFSM P_fsm;
    private PlayerParameter P_parameter;

    public PlayerJumpState(PlayerFSM fsm)
    {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;
    }
    public void OnEnter()
    {
        P_parameter.Player_Animator.Play("Jump");
    }

    public void OnExit()
    {
        P_parameter.Player_Animator.Play("Idle");
    }
    
    public void OnUpdate()
    {
    }
}
public class PlayeAttackState : IPlayerState
{
    private PlayerFSM P_fsm;
    private PlayerParameter P_parameter;

    public PlayeAttackState(PlayerFSM fsm)
    {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;

    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}

public class PlayerAttackState : IPlayerState
{
    private PlayerFSM P_fsm;
    private PlayerParameter P_parameter;
    private AnimatorStateInfo info;
    bool is_Create;
    public PlayerAttackState(PlayerFSM fsm)
    {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;

    }
    public void OnEnter()
    {
        is_Create = false;
        P_parameter.Player_Animator.Play("Attack");
    }

    public void OnExit()
    {
        is_Create = true;
    }

    public void OnUpdate()
    {
        info = P_parameter.Player_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.45f && !is_Create && info.normalizedTime <= 0.55f)
        {
            is_Create = true;
        }
        if (info.normalizedTime >.95f)
        {
            P_fsm.TransitionState(PlayerStateType.idle);
        }
    }
}
public class PlayerSkillState : IPlayerState
{
    private PlayerFSM P_fsm;
    private PlayerParameter P_parameter;
    private AnimatorStateInfo info;

    public PlayerSkillState(PlayerFSM fsm)
    {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;

    }
    public void OnEnter()
    {
        P_parameter.Player_Animator.Play("Skill");
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        info = P_parameter.Player_Animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime > .95f)
        {
            P_fsm.TransitionState(PlayerStateType.idle);
        }
    }
}
public class PlayeHitState : IPlayerState
{
    private PlayerFSM P_fsm;
    private PlayerParameter P_parameter;
    private AnimatorStateInfo info;
    public PlayeHitState(PlayerFSM fsm)
    {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;
        

    }
    public void OnEnter()
    {
        P_parameter.Player_Animator.Play("Hit");
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        info = P_parameter.Player_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >.95f)
        {
            P_fsm.TransitionState(PlayerStateType.idle);
        }
    }
}
public class PlayeDeadState : IPlayerState
{
    private PlayerFSM P_fsm;
    private PlayerParameter P_parameter;

    public PlayeDeadState(PlayerFSM fsm)
    {

        this.P_fsm = fsm;
        this.P_parameter = fsm.playerParameter;

    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}