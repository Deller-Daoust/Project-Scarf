using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUnderattack : PBase
{
    private PFSM PFSM;
    private PDSetting PDSetting;
    private AnimatorStateInfo stateInfo;

    public PUnderattack(PFSM pFSM)
    {
        this.PFSM = pFSM;
        this.PDSetting = pFSM.PDSetting;
    }

    public void OnEnter()
    {
        //PDSetting.animator.Play("PDUnderattack");
        //PDSetting.BeAttacked = false;
    }

    public void OnUpdate()
    {
        stateInfo = PDSetting.animator.GetCurrentAnimatorStateInfo(0);
        if (PDSetting.health<=0)
        {
            PFSM.SwitchState(PunchState.Death);
        }
        if (stateInfo.normalizedTime>=0.95f)
        {
            PDSetting.FindPlayer = GameObject.FindWithTag("Player").transform;
            PFSM.SwitchState(PunchState.Chase);
        }
    }

    public void OnExit()
    {
        PDSetting.getHit = false;
    }
}
