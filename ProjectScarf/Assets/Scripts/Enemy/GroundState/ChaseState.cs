using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{

    private FSM GroundEnemy;
    private EnemySetting EnemySetting;

    public ChaseState(FSM GroundEnemy)
    {
        this.GroundEnemy = GroundEnemy;
        this.EnemySetting = GroundEnemy.enemySetting;
        
    }
   public void OnEnter()
    {
        EnemySetting.animator.Play("chase");
    }

    public void OnUpdate()
    {
        
        GroundEnemy.ChangeDirection(EnemySetting.FindPlayer);
        if (EnemySetting.BeAttacked == true)
        {
            GroundEnemy.SwitchState(StateType.UnderAttack);
        }
        if (EnemySetting.FindPlayer)
        {
            GroundEnemy.transform.position = Vector2.MoveTowards(GroundEnemy.transform.position, EnemySetting.FindPlayer.position, EnemySetting.chaseSpeed * Time.deltaTime);
        }
        if (EnemySetting.FindPlayer == null || GroundEnemy.transform.position.x < EnemySetting.chaseRange[0].position.x || GroundEnemy.transform.position.x > EnemySetting.chaseRange[1].position.x)
        {
            GroundEnemy.SwitchState(StateType.Idle);
        }
        if (Physics2D.OverlapCircle(EnemySetting.AttackRange.position,EnemySetting.AttackNumber,EnemySetting.layerMask))
        {
            GroundEnemy.SwitchState(StateType.Attack);
        }
    }


	
    public void OnExit()
    {

    }
}
