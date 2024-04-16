using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyType
{
    Normal,
    Middle,
    Boss,
    Boss2

}
public enum EnemyStateType
{
    idle,
    Patrol,
    Chase,
    React,
    Hit,
    Dead,
    Attack
}
[Serializable]
public class EnemyParameter
{

    public EnemyType E_Type;
    public int Enemy_HP;
    public float MoveSpeed;
    public float IdleTime;
    public Transform[] PatrolPoints;
    public Transform[] ChasePoints;
    public Animator Enemy_Animator;
    public NavMeshAgent EnemyAgent;
    public bool IsHit;
    public bool IsOnce;
    public float ChaseSpeed;
    public int Enemy_EXP;
    public float Attack_Distance;

    public GameObject Boss_Health;

    //追击距离
    public float ChaseDistence;
    //怪物攻击力
    public int Attack_Num;
    public GameObject Self_Partical;
    //传值 Player造成的伤害
    public int Hit_Damage;
    //玩家
    public Transform Target;
    public float GiveUpChaseDistence;
    public bool is_Dead;
    public bool is_Attack;
    public string Boss_Skill;
    public GameObject Fire;
    public int AttackCount;
}
public class EnemyFSM : MonoBehaviour
{
    public EnemyParameter enemyParameter;
    public IEnemyState currentState;
    private Dictionary<EnemyStateType, IEnemyState> EnemyStates = new Dictionary<EnemyStateType, IEnemyState>();
    // Start is called before the first frame update
    void Start()
    {
        EnemyStates.Add(EnemyStateType.idle, new EnemyIdleState(this));
        EnemyStates.Add(EnemyStateType.Patrol, new EnemyPatrolState(this));
        EnemyStates.Add(EnemyStateType.Chase, new EnemyChaseState(this));
        EnemyStates.Add(EnemyStateType.Attack, new EnemyAttackState(this));
        EnemyStates.Add(EnemyStateType.Hit, new EnemyHitState(this));
        EnemyStates.Add(EnemyStateType.Dead, new EnemyDeadState(this));
        enemyParameter.EnemyAgent = GetComponent<NavMeshAgent>();
        enemyParameter.Enemy_Animator = GetComponent<Animator>();
        enemyParameter.is_Attack = false;
        switch (enemyParameter.E_Type)
        {
            case EnemyType.Normal:
                enemyParameter.Enemy_HP = 100;
                enemyParameter.Attack_Num = 5;
                enemyParameter.ChaseDistence = 9;
                enemyParameter.GiveUpChaseDistence = 10;
                enemyParameter.Attack_Distance = 2f;
                break;
            case EnemyType.Middle:
                enemyParameter.Enemy_HP = 100;
                enemyParameter.Attack_Num = 10;
                enemyParameter.ChaseDistence = 9;
                enemyParameter.GiveUpChaseDistence = 10;
                enemyParameter.Attack_Distance = 4f;
                break;
            case EnemyType.Boss:
                enemyParameter.Enemy_HP = 400;
                enemyParameter.Attack_Num = 30;
                enemyParameter.ChaseDistence = 19;
                enemyParameter.GiveUpChaseDistence = 21;
                enemyParameter.Attack_Distance = 7f;
                enemyParameter.Boss_Health = GameObject.Find("BossCanvas").transform.GetChild(0).gameObject;
                enemyParameter.AttackCount = 0;
                break;
            case EnemyType.Boss2:
                enemyParameter.Enemy_HP = 500;
                enemyParameter.Attack_Num = 30;
                enemyParameter.ChaseDistence = 19;
                enemyParameter.GiveUpChaseDistence = 21;
                enemyParameter.Attack_Distance = 7f;
                enemyParameter.Boss_Health = GameObject.Find("BossCanvas").transform.GetChild(0).gameObject;
                Debug.Log(enemyParameter.Boss_Health);

                break;
            default:
                break;
        }
        enemyParameter.Self_Partical = transform.GetChild(1).gameObject;
        enemyParameter.IdleTime = 3f;
        enemyParameter.ChaseSpeed = 2;
        enemyParameter.IsOnce = true;
        enemyParameter.IsHit = false;
        currentState = EnemyStates[EnemyStateType.idle];
        TransitionState(EnemyStateType.idle);
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, 0)) < enemyParameter.ChaseDistence && enemyParameter.IsOnce)
        {

            if (enemyParameter.E_Type.Equals(EnemyType.Boss) || enemyParameter.E_Type.Equals(EnemyType.Boss2))
            {
                enemyParameter.Boss_Health.SetActive(true);
            }
            enemyParameter.Target = GameObject.FindGameObjectWithTag("Player").transform;
            TransitionState(EnemyStateType.Chase);
            enemyParameter.IsOnce = false;
        }
        if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, 0)) > enemyParameter.GiveUpChaseDistence && !enemyParameter.IsOnce)
        {

            if (enemyParameter.E_Type.Equals(EnemyType.Boss) || enemyParameter.E_Type.Equals(EnemyType.Boss2))
            {
                enemyParameter.Boss_Health.SetActive(false);
            }
            enemyParameter.Target = null;
            enemyParameter.IsOnce = true;
            TransitionState(EnemyStateType.idle);
        }
        currentState.OnUpdate();
    }

    void Destroy_Self()
    {
        Destroy(gameObject);
    }
    public void Deastroy_S()
    {
        Invoke("Destroy_Self", 1);
    }

    public void Show_F()
    {
        StartCoroutine(Show_Fire());

    }
    public IEnumerator Show_Fire()
    {

        yield return new WaitForSeconds(2);
        enemyParameter.Fire.SetActive(true);

    }
    public void TransitionState(EnemyStateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        if (currentState != EnemyStates[EnemyStateType.Dead])
        {
            currentState = EnemyStates[type];
            currentState.OnEnter();
        }

    }
}
