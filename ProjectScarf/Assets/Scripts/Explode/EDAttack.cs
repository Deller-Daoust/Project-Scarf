using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDAttack : EBase
{
    private EFSM EFSM;
    private EDSetting EDSetting;
    private AnimatorStateInfo stateInfo;
    public EDAttack(EFSM eFSM)
    {
        this.EFSM = eFSM;
        this.EDSetting = eFSM.EDSetting;
    }

    public void OnEnter()
    {
        EDSetting.animator.Play("EDAttack");
        EFSM.gameObject.GetComponent<AudioSource>().Stop();
        EFSM.gameObject.GetComponent<AudioSource>().PlayOneShot(EFSM.boomSound);
    }

    public void OnUpdate()
    {
        stateInfo = EDSetting.animator.GetCurrentAnimatorStateInfo(0);
        if (EDSetting.BeAttacked == true)
        {
            EFSM.SwitchState(ExplodeState.UnderAttack);
        }
        if (stateInfo.normalizedTime >= 0.95f)
        {
            EFSM.Destroy(EFSM.gameObject);
        }
    }

    public void OnExit()
    {

    }
}
