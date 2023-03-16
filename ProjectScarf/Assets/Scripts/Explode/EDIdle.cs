using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDIdle : EBase
{
    private EFSM EFSM;
    private EDSetting EDSetting;
    private float timer;
    public EDIdle(EFSM eFSM)
    {
        this.EFSM = eFSM;
        this.EDSetting = eFSM.EDSetting;
    }

    public void OnEnter()
    {
        EDSetting.animator.Play("EDIdle");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (EDSetting.BeAttacked == true)
        {
            EFSM.SwitchState(ExplodeState.UnderAttack);
        }
        if (EDSetting.FindPlayer != null && EDSetting.FindPlayer.position.x >= EDSetting.ChaseRange[0].position.x && EDSetting.FindPlayer.position.x <= EDSetting.ChaseRange[1].position.x)
        {
            EFSM.SwitchState(ExplodeState.Chase);
        }
        if (timer >= EDSetting.idleTime)
        {
            EFSM.SwitchState(ExplodeState.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;
    }
}
