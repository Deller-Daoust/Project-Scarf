using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ShootState
{
    Idle,Patrol,Chase,UnderAttack,Attack,Death
}


[Serializable]
public class ShootSetting
{
    public int health;
    public float damage, moveSpeed, ChaseSpeed, idleTime,AttackNumber;
    public Transform[] PatrolRange, ChaseRange;
    public Transform FindPlayer, AttackRange;
    public GameObject bullets;
    public Animator animator;

    public LayerMask LayerMask;
    public bool getHit;
    public bool BeAttacked;
    
}
public class ShootFSM : MonoBehaviour
{
    public ShootSetting shootSetting;
    private ShootBase currentState;
    private Dictionary<ShootState, ShootBase> state = new Dictionary<ShootState, ShootBase>();
    // Start is called before the first frame update
    void Start()
    {
        state.Add(ShootState.Idle, new ShootIdle(this));
        state.Add(ShootState.Patrol, new ShootPartol(this));
        state.Add(ShootState.UnderAttack, new ShootUnderAttack(this));
        state.Add(ShootState.Attack, new ShootAttack(this));
        state.Add(ShootState.Chase, new ShootChase(this));
        state.Add(ShootState.Death, new ShootDie(this));
        SwitchState(ShootState.Idle);
        shootSetting.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();

        if (Input.GetMouseButtonDown(1))
        {
            shootSetting.getHit = true;
        }
        if (shootSetting.BeAttacked)
        {
            shootSetting.animator.Play("Underattack", -1, 0f);
            SwitchState(ShootState.UnderAttack);
            shootSetting.BeAttacked = false;
        }
    }
    public void SwitchState(ShootState type)
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
            if (transform.position.x > trans.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < trans.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shootSetting.FindPlayer = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shootSetting.FindPlayer = null;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(shootSetting.AttackRange.position, shootSetting.AttackNumber);
    }

    public void Shooting()
    {
        GameObject bullet = Instantiate(shootSetting.bullets, new Vector2(transform.position.x + (transform.localScale.x * 0.6f), transform.position.y + 0.9f), Quaternion.identity);
        BulletPrefab bulletPrefab = bullet.GetComponent<BulletPrefab>();
        //Vector2 Dir;
        if (transform.localScale.x == -1)
        {
        //    Dir = new Vector2(-1, 0);
        //    bulletPrefab.Shoot(Dir, 500);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * 12f;
        }
        else if (transform.localScale.x == 1)
        {
        //    Dir = new Vector2(1, 0);
        //    bulletPrefab.Shoot(Dir, 500);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * 12f;
        }
    }
}
