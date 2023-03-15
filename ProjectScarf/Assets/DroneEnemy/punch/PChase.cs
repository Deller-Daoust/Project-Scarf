using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PChase : PBase
{
    private PFSM PFSM;
    private PDSetting PDSetting;
    
    public PChase(PFSM pFSM)
    {
        this.PFSM = pFSM;
        this.PDSetting = pFSM.PDSetting;
    }

    public void OnEnter()
    {
        PDSetting.animator.Play("PDChase");
    }

    public void OnUpdate()
    {

        PFSM.ChangeDirection(PDSetting.FindPlayer);
        if (PDSetting.BeAttacked == true)
        {
            PFSM.SwitchState(PunchState.UnderAttack);
        }
        if (PDSetting.FindPlayer)
        {
            PFSM.transform.position = Vector2.MoveTowards(PFSM.transform.position, PDSetting.FindPlayer.position +new Vector3(0f,1f,0f), PDSetting.ChaseSpeed * Time.deltaTime);
        }
        if (PDSetting.FindPlayer == null || PFSM.transform.position.x < PDSetting.ChaseRange[0].position.x || PFSM.transform.position.x > PDSetting.ChaseRange[1].position.x)
        {
            PFSM.SwitchState(PunchState.Idle);
        }
        if (Physics2D.OverlapCircle(PDSetting.AttackRange.position, PDSetting.AttackNumber, PDSetting.LayerMask))
        {
            PDSetting.canAttack = true;
            PFSM.SwitchState(PunchState.Attack);
        }
    }

    public void OnExit()
    {

    }
}
