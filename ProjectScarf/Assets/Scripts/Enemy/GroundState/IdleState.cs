using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public Sword Sword;
    private FSM GroundEnemy;
    private EnemySetting EnemySetting;

    private float timer;
    public IdleState(FSM GroundEnemy)
    {
        this.GroundEnemy = GroundEnemy;
        this.EnemySetting = GroundEnemy.enemySetting;
        
    }
   public void OnEnter()
    {
        EnemySetting.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (EnemySetting.BeAttacked == true)
        {
            GroundEnemy.SwitchState(StateType.UnderAttack);
        }
        if (EnemySetting.FindPlayer != null && EnemySetting.FindPlayer.position.x >= EnemySetting.chaseRange[0].position.x && EnemySetting.FindPlayer.position.x <= EnemySetting.chaseRange[1].position.x)
        {
            GroundEnemy.SwitchState(StateType.Chase);
        }
        if (timer >=EnemySetting.idleTime)
        {
            GroundEnemy.SwitchState(StateType.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;
    }
}
