using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyFSM fsm;
    private EnemyParameter parameter;
    //巡逻时间间隔
    private float timer;

    public EnemyIdleState(EnemyFSM fsm)
    {
        this.fsm = fsm;
        this.parameter = fsm.enemyParameter;
    }
    public void OnEnter()
    {
        if (parameter.E_Type.Equals(EnemyType.Boss))
        {

        }
        parameter.Enemy_Animator.Play("Idle");
        parameter.EnemyAgent.SetDestination(fsm.transform.position);
    }

    public void OnExit()
    {
        timer = 0;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (parameter.IsHit)
        {
            fsm.TransitionState(EnemyStateType.Hit);
        }
        if (timer > parameter.IdleTime)
        {
            fsm.TransitionState(EnemyStateType.Patrol);
        }
    }
}
public class EnemyPatrolState : IEnemyState
{
    private EnemyFSM fsm;
    private EnemyParameter parameter;
    private int partrolPosition;
    public EnemyPatrolState(EnemyFSM fsm)
    {

        this.fsm = fsm;
        this.parameter = fsm.enemyParameter;
    }
    public void OnEnter()
    {


        parameter.Enemy_Animator.Play("Walk");
        fsm.transform.LookAt(parameter.PatrolPoints[partrolPosition].position);
    }

    public void OnExit()
    {
        partrolPosition++;
        if (partrolPosition >= parameter.PatrolPoints.Length)
        {
            partrolPosition = 0;
        }
    }

    public void OnUpdate()
    {
        parameter.EnemyAgent.SetDestination(parameter.PatrolPoints[partrolPosition].position);
        if (parameter.IsHit)
        {
            fsm.TransitionState(EnemyStateType.Hit);
        }
        if (Vector3.Distance(fsm.transform.position, parameter.PatrolPoints[partrolPosition].position) < 1.2f)
        {
            fsm.TransitionState(EnemyStateType.idle);
        }
    }
}
public class EnemyChaseState : IEnemyState
{
    private EnemyFSM fsm;
    private EnemyParameter parameter;
    public EnemyChaseState(EnemyFSM fsm)
    {

        this.fsm = fsm;
        this.parameter = fsm.enemyParameter;
    }
    public void OnEnter()
    {

        parameter.EnemyAgent.speed = fsm.enemyParameter.ChaseSpeed;
        parameter.Enemy_Animator.Play("Run");
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (parameter.IsHit)
        {
            fsm.TransitionState(EnemyStateType.Hit);
        }

        if (parameter.Target != null)
        {
            parameter.EnemyAgent.SetDestination(parameter.Target.position);
            if (Vector3.Distance(parameter.Target.position, fsm.transform.position) < parameter.Attack_Distance)
            {
                fsm.TransitionState(EnemyStateType.Attack);
            }
        }

;
    }
}

public class EnemyHitState : IEnemyState
{
    private EnemyFSM fsm;
    private EnemyParameter parameter;
    private AnimatorStateInfo info;
    public EnemyHitState(EnemyFSM fsm)
    {

        this.fsm = fsm;
        this.parameter = fsm.enemyParameter;
    }
    public void OnEnter()
    {
        parameter.Enemy_Animator.Play("Hit");
        parameter.EnemyAgent.SetDestination(fsm.transform.position);
        parameter.Enemy_HP -= parameter.Hit_Damage;
    }
    public void OnUpdate()
    {
        parameter.Target = GameObject.FindGameObjectWithTag("Player").transform;
        info = parameter.Enemy_Animator.GetCurrentAnimatorStateInfo(0);
        if (parameter.Enemy_HP <= 0)
        {
            fsm.TransitionState(EnemyStateType.Dead);
        }

        if (info.normalizedTime >= .95f)
        {
            fsm.TransitionState(EnemyStateType.Chase);
        }

    }

    public void OnExit()
    {
        parameter.IsHit = false;
        fsm.enemyParameter.Self_Partical.SetActive(false);
    }


}
public class EnemyDeadState : IEnemyState
{
    private EnemyFSM fsm;
    private EnemyParameter parameter;
    private AnimatorStateInfo info;
    public EnemyDeadState(EnemyFSM fsm)
    {

        this.fsm = fsm;
        this.parameter = fsm.enemyParameter;
    }
    public void OnEnter()
    {
        parameter.Enemy_Animator.Play("Dead");
        parameter.EnemyAgent.SetDestination(fsm.transform.position);
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {

        info = parameter.Enemy_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > .95f)
        {
            fsm.Deastroy_S();
        }
    }

}
public class EnemyAttackState : IEnemyState
{
    private EnemyFSM fsm;
    private EnemyParameter parameter;
    private AnimatorStateInfo info;
    string Skill;
    public EnemyAttackState(EnemyFSM fsm)
    {

        this.fsm = fsm;
        this.parameter = fsm.enemyParameter;
    }
    public void OnEnter()
    {

        fsm.transform.LookAt(parameter.Target);
        parameter.Enemy_Animator.Play("Attack");
        parameter.EnemyAgent.SetDestination(fsm.transform.position);
    }

    public void OnExit()
    {
        parameter.is_Attack = false;
        parameter.Boss_Skill = "";
        PlayerHealthSys.Instance.isPlayer_Hit = false;
    }

    public void OnUpdate()
    {
        if (parameter.IsHit)
        {
            fsm.TransitionState(EnemyStateType.Hit);
        }
        info = parameter.Enemy_Animator.GetCurrentAnimatorStateInfo(0);
        float diss = Vector3.Distance(fsm.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        float angle = Vector3.Angle(fsm.transform.forward, GameObject.FindGameObjectWithTag("Player").transform.position - fsm.transform.position);
        if (angle < 90 && diss < parameter.Attack_Distance && info.normalizedTime > .55f && !parameter.is_Attack)
        {
            parameter.is_Attack = true;
            EventManager.Instance.TriggerEvent<int>("Player_Hit", parameter.Attack_Num);
        }
        if (info.normalizedTime > .95f)
        {
            fsm.TransitionState(EnemyStateType.Chase);
        }

    }
}

