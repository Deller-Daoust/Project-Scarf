using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderAttackState : BaseState
{
    private FSM GroundEnemy;
    private EnemySetting EnemySetting;
    private AnimatorStateInfo stateInfo;
    
    public UnderAttackState(FSM GroundEnemy)
    {
        this.GroundEnemy = GroundEnemy;
        this.EnemySetting = GroundEnemy.enemySetting;
        
    }
   public void OnEnter()
    {
        EnemySetting.animator.Play("Underattack");
        EnemySetting.health--;
    }

    public void OnUpdate()
    {
        stateInfo = EnemySetting.animator.GetCurrentAnimatorStateInfo(0);
        if (EnemySetting.health<=0)
        {
            GroundEnemy.SwitchState(StateType.Death);
        }
        if (stateInfo.normalizedTime >=0.95f)
        {
            EnemySetting.FindPlayer = GameObject.FindWithTag("Player").transform;
            GroundEnemy.SwitchState(StateType.Chase);
        }
       
        
    }

    public void OnExit()
    {
        EnemySetting.getHit = false;
    }
}
