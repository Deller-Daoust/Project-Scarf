using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
 
    private FSM GroundEnemy;
    private EnemySetting EnemySetting;

    private AnimatorStateInfo stateInfo;
    public AttackState(FSM GroundEnemy)
    {
        this.GroundEnemy = GroundEnemy;
        this.EnemySetting = GroundEnemy.enemySetting;
        
    }
   public void OnEnter()
    {
        EnemySetting.animator.Play("Attack");
    }

    public void OnUpdate()
    {
        stateInfo = EnemySetting.animator.GetCurrentAnimatorStateInfo(0);
        if (EnemySetting.BeAttacked == true)
        {
            GroundEnemy.SwitchState(StateType.UnderAttack);
        }
        if (stateInfo.normalizedTime>=0.95f)
        {
            GroundEnemy.SwitchState(StateType.Chase);
        }
    }

    public void OnExit()
    {

    }
}
