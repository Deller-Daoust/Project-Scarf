using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum ExplodeState{ 

    Idle,Patrol,Chase,UnderAttack,Attack,Death
}

[Serializable]
public class EDSetting
{
    public int health;
    public float damage, moveSpeed, ChaseSpeed, idleTime, AttackNumber;
    public Transform[] PatrolRange, ChaseRange;
    public Transform FindPlayer, AttackRange;

    public Animator animator;

    public LayerMask LayerMask;
    public bool getHit;
    public bool BeAttacked;
    public bool canAttack=true;
    public Transform NextPosition;

    public float StartTime;
    public float Times;


}

public class EFSM : MonoBehaviour
{
    public AnimatorStateInfo stateInfo;
    public EDSetting EDSetting;
    private EBase currentState;
    public AudioClip boomSound;
    private Dictionary<ExplodeState, EBase> state = new Dictionary<ExplodeState, EBase>();
    void Start()
    {
        state.Add(ExplodeState.Idle, new EDIdle(this));
        state.Add(ExplodeState.Patrol, new EDPatrol(this));
        state.Add(ExplodeState.UnderAttack, new EDUnderattack(this));
        state.Add(ExplodeState.Attack, new EDAttack(this));
        state.Add(ExplodeState.Chase, new EDChase(this));
        state.Add(ExplodeState.Death, new EDDie(this));
        SwitchState(ExplodeState.Idle);
        EDSetting.animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        currentState.OnUpdate();
        if (currentState==state[ExplodeState.UnderAttack])
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >=0.95f && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EDUnderAttack"))
            {
                Destroy(gameObject);
            }
        }
        if (currentState == state[ExplodeState.Attack])
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EDAttack") && EDSetting.canAttack)
            {
                Collider2D player = EDSetting.AttackRange.gameObject.GetComponent<Attack_OverlapCircle>().IsOverlapping();
                if (player != null)
                {
                    if (player.gameObject.transform.parent.GetComponent<Player_Movement>().rolling <= 0)
                    {
                        GetComponent<Hit_Player>().HitPlayer();
                        if (!player.gameObject.transform.parent.GetComponent<Player_Movement>().didGetHit)
                        {
                            EDSetting.BeAttacked = true;

                        }
                    }
                }
                EDSetting.canAttack = false;

            }
        }
    }

    public void SwitchState(ExplodeState type)
    {
        if (currentState !=null)
        {
            currentState.OnExit();
        }

        currentState = state[type];
        currentState.OnEnter();
    }

    public void ChangeDirection(Transform trans)
    {
        if (trans != null)
        {
            if (transform.position.x > EDSetting.NextPosition.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (transform.position.x < EDSetting.NextPosition.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EDSetting.FindPlayer = collision.transform;
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EDSetting.FindPlayer = null;
        }
       
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(EDSetting.AttackRange.position, EDSetting.AttackNumber);
    }

    public void DamageTake(int damage)
    {
        EDSetting.health -= damage;
    }

    public Vector2 GetRandomPos()
    {
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(EDSetting.PatrolRange[0].position.x, EDSetting.PatrolRange[1].position.x), UnityEngine.Random.Range(EDSetting.PatrolRange[0].position.y, EDSetting.PatrolRange[1].position.y));
            return randomPos;
    }

    //public void Shooting()
    //{
    //    GameObject bullet = Instantiate(shootSetting.bullets, transform.position, Quaternion.identity);
    //    BulletPrefab bulletPrefab = bullet.GetComponent<BulletPrefab>();
    //    Vector2 Dir;
    //    if (transform.localScale.x == -5)
    //    {
    //        Dir = new Vector2(-1, 0);
    //        bulletPrefab.Shoot(Dir, 500);
    //    }
    //    else if (transform.localScale.x == 5)
    //    {
    //        Dir = new Vector2(1, 0);
    //        bulletPrefab.Shoot(Dir, 500);
    //    }
    //}
}
