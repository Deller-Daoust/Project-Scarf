using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartolState : BaseState
{

    private FSM GroundEnemy;
    private EnemySetting EnemySetting;

    private int patrolPosition;
    public PartolState(FSM GroundEnemy)
    {
        this.GroundEnemy = GroundEnemy;
        this.EnemySetting = GroundEnemy.enemySetting;
        
    }
   public void OnEnter()
    {
        EnemySetting.animator.Play("Patrol");
    }

    public void OnUpdate()
    {
        
        GroundEnemy.ChangeDirection(EnemySetting.PatrolRange[patrolPosition]);
        GroundEnemy.transform.position = Vector2.MoveTowards(GroundEnemy.transform.position, EnemySetting.PatrolRange[patrolPosition].position, EnemySetting.moveSpeed * Time.deltaTime);

        if (EnemySetting.BeAttacked==true)
        {
            GroundEnemy.SwitchState(StateType.UnderAttack);
        }
        if (EnemySetting.FindPlayer != null && EnemySetting.FindPlayer.position.x >= EnemySetting.chaseRange[0].position.x && EnemySetting.FindPlayer.position.x <= EnemySetting.chaseRange[1].position.x)
        {
            GroundEnemy.SwitchState(StateType.Chase);
        }
        if (Vector2.Distance(GroundEnemy.transform.position, EnemySetting.PatrolRange[patrolPosition].position)<0.1f)
        {
            GroundEnemy.SwitchState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        patrolPosition++;
        if (patrolPosition >= EnemySetting.PatrolRange.Length)
        {
            patrolPosition = 0;
        }
    }
    
    
}
