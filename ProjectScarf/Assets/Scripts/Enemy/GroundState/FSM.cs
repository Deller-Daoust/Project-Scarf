using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,Patrol,Chase,UnderAttack,Attack,Death
}

[Serializable]public class EnemySetting
{

    public int health;
    public float damage;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public Transform[] PatrolRange;
    public Transform[] chaseRange;
    public Transform FindPlayer;
    public Animator animator;

    public LayerMask layerMask;
    public Transform AttackRange;
    public float AttackNumber;

    public bool getHit;
    public bool BeAttacked;
}
public class FSM : MonoBehaviour
{
    //private Sword sword;
    private Combat_System Combat_System;
    public EnemySetting enemySetting;
    private BaseState currentState;
    private Dictionary<StateType, BaseState> states = new Dictionary<StateType, BaseState>();
    // Start is called before the first frame update
     void Start()
    {

        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PartolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.UnderAttack, new UnderAttackState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Death, new DeathState(this));
        SwitchState(StateType.Idle);

        enemySetting.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
     void Update()
    {

        currentState.OnUpdate();

        if (Input.GetMouseButtonDown(1))
        {
            enemySetting.getHit = true;
        }
    }

    public void SwitchState(StateType type)
    {
        if (currentState !=null)
        {
            currentState.OnExit();

        }
        currentState = states[type];
        currentState.OnEnter();
    }

    public void ChangeDirection(Transform trans)
    {
        if (trans != null)
        {
            if (transform.position.x > trans.position.x)
            {
                transform.localScale = new Vector3(-4, 4, 1);
            }
            else if (transform.position.x < trans.position.x)
            {
                transform.localScale = new Vector3(4, 4, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemySetting.FindPlayer = collision.transform;
        }
        if (collision.CompareTag("PlayerSword"))
        {
            enemySetting.BeAttacked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemySetting.FindPlayer = null;
        }
        if (collision.CompareTag("PlayerSword"))
        {
            enemySetting.BeAttacked = false;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemySetting.AttackRange.position, enemySetting.AttackNumber);
    }

    public void DamageTake(int damage)
    {
        enemySetting.health -= damage;
    }
   
}
