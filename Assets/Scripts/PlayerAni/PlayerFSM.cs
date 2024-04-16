using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PlayerStateType
{
    idle,
    //受击
    Hit,
    Dead,
    Walk,
    Jump,
    Skill,
    Run,
    Attack
}
[Serializable]
public class PlayerParameter
{
    public int Player_HP;
    public float MoveSpeed;
    public Animator Player_Animator;
    public float Attack_Distance;
    public PlayerStateType current_StateEnum;
    public bool is_Dead;
}
public class PlayerFSM : MonoBehaviour
{
    public PlayerParameter playerParameter;
    public IPlayerState P_currentState;
    private Dictionary<PlayerStateType, IPlayerState> PlayerState = new Dictionary<PlayerStateType, IPlayerState>();

    private void Start()
    {
        playerParameter.Player_Animator = this.gameObject.GetComponent<Animator>();
        PlayerState.Add(PlayerStateType.idle, new PlayerIdleState(this));
        PlayerState.Add(PlayerStateType.Walk, new PlayerWalkState(this));
        PlayerState.Add(PlayerStateType.Jump, new PlayerJumpState(this));
        PlayerState.Add(PlayerStateType.Skill, new PlayerSkillState(this));
        PlayerState.Add(PlayerStateType.Hit, new PlayeHitState(this));
        PlayerState.Add(PlayerStateType.Dead, new PlayeDeadState(this));
        PlayerState.Add(PlayerStateType.Attack, new PlayerAttackState(this));
        P_currentState = PlayerState[PlayerStateType.idle];
        playerParameter.current_StateEnum = PlayerStateType.idle;
        TransitionState(PlayerStateType.idle);
    }

    private void Update()
    {
        P_currentState.OnUpdate();
    }

    public void TransitionState(PlayerStateType type)
    {
        if (P_currentState != null)
        {

            P_currentState.OnExit();
        }
        playerParameter.current_StateEnum = type;
        P_currentState = PlayerState[type];
        P_currentState.OnEnter();
    }
}
