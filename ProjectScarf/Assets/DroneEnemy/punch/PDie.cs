using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDie : PBase
{
    private PFSM PFSM;
    private PDSetting PDSetting;
    private AnimatorStateInfo stateInfo;
    public PDie(PFSM pFSM)
    {
        this.PFSM = pFSM;
        this.PDSetting = pFSM.PDSetting;
    }

    public void OnEnter()
    {
        PDSetting.animator.Play("PDDie");
    }

    public void OnUpdate()
    {
        stateInfo = PDSetting.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 0.95f)
        {
            PFSM.Destroy(PFSM.gameObject);
        }
    }

    public void OnExit()
    {

    }
}
