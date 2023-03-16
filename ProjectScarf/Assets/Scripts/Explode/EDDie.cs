using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDDie :EBase
{
    private EFSM EFSM;
    private EDSetting EDSetting;

    public EDDie(EFSM eFSM)
    {
        this.EFSM = eFSM;
        this.EDSetting = eFSM.EDSetting;
    }

    public void OnEnter()
    {
        EDSetting.animator.Play("EDDie");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
