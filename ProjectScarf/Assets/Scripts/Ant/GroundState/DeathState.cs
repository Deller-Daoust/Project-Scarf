using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState :BaseState
{
    private FSM GroundEnemy;
    private EnemySetting EnemySetting;
   
    public DeathState(FSM GroundEnemy)
    {
        this.GroundEnemy = GroundEnemy;
        this.EnemySetting = GroundEnemy.enemySetting;

    }
    public void OnEnter()
    {
        EnemySetting.animator.Play("Die");
    }

    public void OnUpdate()
    {
        

    }

    public void OnExit()
    {

    }
}
