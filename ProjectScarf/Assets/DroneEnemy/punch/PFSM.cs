using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PunchState
{
    Idle, Patrol, Chase, UnderAttack, Attack, Death
}

[Serializable]
public class PDSetting
{
    public int health;
    public float damage, moveSpeed, ChaseSpeed, idleTime, AttackNumber;
    public Transform[] PatrolRange, ChaseRange;
    public Transform FindPlayer, AttackRange;

    public Animator animator;

    public LayerMask LayerMask;
    public bool getHit;
    public bool BeAttacked;
    public bool canAttack;
    public Transform NextPosition;

    public float StartTime;
    public float Times;
}
public class PFSM : MonoBehaviour
{
    public AnimatorStateInfo stateInfo;
    public PDSetting PDSetting;
    private PBase currentState;
    private Dictionary<PunchState, PBase> state = new Dictionary<PunchState, PBase>();

    void Start()
    {
        state.Add(PunchState.Idle, new PIdle(this));
        state.Add(PunchState.Patrol, new PPatrol(this));
        state.Add(PunchState.UnderAttack, new PUnderattack(this));
        state.Add(PunchState.Attack, new PAttack(this));
        state.Add(PunchState.Chase, new PChase(this));
        state.Add(PunchState.Death, new PDie(this));
        SwitchState(PunchState.Patrol);
        PDSetting.animator = GetComponent<Animator>();
    }

    void Update()
    {
        PDSetting.health = gameObject.GetComponent<HP_Handler>().health;
        currentState.OnUpdate();

        if (PDSetting.health<=2)
        {
            SwitchState(PunchState.Death);
        }

        if (currentState == state[PunchState.Attack])
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >=0.4f && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PDAttack") && PDSetting.canAttack)
            {
                Collider2D player = PDSetting.AttackRange.gameObject.GetComponent<Attack_OverlapCircle>().IsOverlapping();
                if (player != null)
                {
                    if (player.gameObject.transform.parent.GetComponent<Player_Movement>().rolling <= 0)
                    {
                        GetComponent<Hit_Player>().HitPlayer();
                        if (!player.gameObject.transform.parent.GetComponent<Player_Movement>().didGetHit)
                        {
                            PDSetting.BeAttacked = true;

                        }
                    }
                }
                PDSetting.canAttack = false;

            }
        }
    }

    public void SwitchState(PunchState type)
    {
        if (currentState != null)
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
            if (transform.position.x > PDSetting.NextPosition.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < PDSetting.NextPosition.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PDSetting.FindPlayer = collision.transform;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           PDSetting.FindPlayer = null;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PDSetting.AttackRange.position, PDSetting.AttackNumber);
    }

   
    public void DamageTake(int damage)
    {
        PDSetting.health -= damage;
    }

    public Vector2 GetRandomPos()
    {
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(PDSetting.PatrolRange[0].position.x, PDSetting.PatrolRange[1].position.x), UnityEngine.Random.Range(PDSetting.PatrolRange[0].position.y, PDSetting.PatrolRange[1].position.y));
        return randomPos;
    }
}
