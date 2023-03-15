using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDUnderattack : EBase
{
    private EFSM EFSM;
    private EDSetting EDSetting;
    private AnimatorStateInfo stateInfo;
    public EDUnderattack(EFSM eFSM)
    {
        this.EFSM = eFSM;
        this.EDSetting = eFSM.EDSetting;
    }

    public void OnEnter()
    {
        EDSetting.animator.Play("EDUnderattack");
        
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
