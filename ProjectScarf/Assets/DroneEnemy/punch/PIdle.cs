using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIdle : PBase
{
    private PFSM PFSM;
    private PDSetting PDSetting;
    private float timer;
    public PIdle(PFSM pFSM)
    {
        this.PFSM = pFSM;
        this.PDSetting = pFSM.PDSetting;
    }

    public void OnEnter()
    {
        PDSetting.animator.Play("PDIdle");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (PDSetting.BeAttacked == true)
        {
            PFSM.SwitchState(PunchState.UnderAttack);
        }
        if (PDSetting.FindPlayer != null && PDSetting.FindPlayer.position.x >= PDSetting.ChaseRange[0].position.x && PDSetting.FindPlayer.position.x <= PDSetting.ChaseRange[1].position.x)
        {
            PFSM.SwitchState(PunchState.Chase);
        }
        if (timer >= PDSetting.idleTime)
        {
            PFSM.SwitchState(PunchState.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;
    }
}
