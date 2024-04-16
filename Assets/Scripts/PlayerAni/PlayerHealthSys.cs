using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerHealthSys : Singleton<PlayerHealthSys>
{

    public int KillEnemy_All;
    public int KillEnemy_Task;
    private float player_HP;
    private float Player_HP_MAX;

    private PlayerFSM P_fsm;

    public float regenTime = 5f;
    public float Timer;
    public bool isBoss_Fire;
    //角色是否还在受攻击
    public bool isPlayer_Hit;
    //任务索引
    public int Task_index;
    //攻击距离
    private float AttackDistance_Q;
    private float AttackDistance_G;
    public int Player_ATK_J;
    public int Player_ATK_K;

    public float Player_HP
    {
        get => player_HP; set
        {
            if (player_HP != value)
            {
                player_HP = value;
                UltimateStatusBar.UpdateStatus("Player", "Health", value, Player_HP_MAX);
            }
        }
    }


    public void Player_Strength()
    {
        Player_ATK_J = 30;
        Player_ATK_K = 55;
        Player_HP_MAX = 200;
        Player_HP = Player_HP_MAX;
    }
    // Start is called before the first frame update
    void Start()
    {
        isBoss_Fire = false;
        Player_HP_MAX = 100;
        Player_HP = Player_HP_MAX;
        AttackDistance_Q = 8;
        AttackDistance_G = 14;
        Player_ATK_J = 15;
        Player_ATK_K = 30;
        P_fsm = GetComponent<PlayerFSM>();
        EventManager.Instance.AddEventListener<int>("Player_Hit", Player_Hit);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEvent<int>("Player_Hit", Player_Hit);
    }
    // Update is called once per frame
    void Update()
    {


        if (!isPlayer_Hit)
        {
            if (Player_HP < Player_HP_MAX)
            {
                regenTime -= Time.deltaTime;
                if (regenTime <= 0)
                {
                    Player_HP += 10;
                    regenTime = 1;
                }
            }
        }
        else
        {
            regenTime = 5;
        }
    }



    void Restore()
    {

    }

    /// <summary>
    /// 角色受伤脚本
    /// </summary>
    void Player_Hit(int Hit)
    {
        isPlayer_Hit = true;
        if (Hit >= Player_HP)
        {
            UIMgr.Instance.Player_EffectMusic(UIMgr.Instance.Hit);
            Player_HP = 0;
            Player_Dead();
        }
        else
        {

            Player_HP -= Hit;
        }
    }
    /// <summary>
    /// 角色死亡
    /// </summary>
    void Player_Dead()
    {
        UIMgr.Instance.FailGame();
    }
    /// <summary>
    /// 角色攻击脚本
    /// </summary>
    IEnumerator Optimization_K(EnemyFSM fsm)
    {
        if (!isBoss_Fire)
        {
            yield return new WaitForSeconds(0.3f);
            fsm.TransitionState(EnemyStateType.Hit);
            yield return new WaitForSeconds(0.8f);
            fsm.TransitionState(EnemyStateType.Hit);
        }
        // 在合适的时候释放协程
        StopCoroutine(Optimization_K(fsm));
    }
    IEnumerator Optimization_J(EnemyFSM fsm)
    {
        if (!isBoss_Fire)
        {
            fsm.TransitionState(EnemyStateType.Hit);
            yield return new WaitForSeconds(0.6f);
            fsm.TransitionState(EnemyStateType.Hit);
        }
        StopCoroutine(Optimization_J(fsm));
    }



}
