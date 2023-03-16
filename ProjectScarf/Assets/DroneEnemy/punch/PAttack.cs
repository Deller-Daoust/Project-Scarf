using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAttack : PBase
{
    private PFSM PFSM;
    private PDSetting PDSetting;
    private AnimatorStateInfo stateInfo;
    public PAttack(PFSM pFSM)
    {
        this.PFSM = pFSM;
        this.PDSetting = pFSM.PDSetting;
    }

    public void OnEnter()
    {
        PDSetting.animator.Play("PDAttack");
    }

    public void OnUpdate()
    {
        stateInfo = PDSetting.animator.GetCurrentAnimatorStateInfo(0);
        if (PDSetting.BeAttacked == true)
        {
            PFSM.SwitchState(PunchState.UnderAttack);
        }
        if (stateInfo.normalizedTime >=0.95f)
        {
            PFSM.SwitchState(PunchState.Chase);
        }
    }

    public void OnExit()
    {

    }
}
