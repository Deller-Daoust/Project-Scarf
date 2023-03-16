using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPatrol : PBase
{
    private PFSM PFSM;
    private PDSetting PDSetting;
    private int patrolPosition;

    public PPatrol(PFSM pFSM)
    {
        this.PFSM = pFSM;
        this.PDSetting = pFSM.PDSetting;
    }

    public void OnEnter()
    {
        PDSetting.Times = PDSetting.StartTime;
        PDSetting.animator.Play("PDPatrol");
        PDSetting.NextPosition.position = PFSM.GetRandomPos();
    }

    public void OnUpdate()
    {
        PFSM.ChangeDirection(PDSetting.PatrolRange[patrolPosition]);
        if (PDSetting.Times < -1)
        {
            PDSetting.Times = PDSetting.StartTime;
        }
        PFSM.transform.position = Vector2.MoveTowards(PFSM.transform.position, PDSetting.NextPosition.position, PDSetting.moveSpeed * Time.deltaTime);
        if (Vector2.Distance(PFSM.transform.position, PDSetting.NextPosition.position) < 0.1f)
        {
            if (PDSetting.Times <= 0)
            {
                PDSetting.NextPosition.position = PFSM.GetRandomPos();
                PDSetting.Times = PDSetting.StartTime;
            }
            else
            {
                PDSetting.Times -= Time.deltaTime;
            }
        }

        if (PDSetting.BeAttacked == true)
        {
            PFSM.SwitchState(PunchState.UnderAttack);
        }
        if (PDSetting.FindPlayer != null && PDSetting.FindPlayer.position.x >= PDSetting.ChaseRange[0].position.x && PDSetting.FindPlayer.position.x <= PDSetting.ChaseRange[1].position.x)
        {
            PFSM.SwitchState(PunchState.Chase);
        }
        if (Vector2.Distance(PFSM.transform.position, PDSetting.PatrolRange[patrolPosition].position) < 0.1f)
        {
            PFSM.SwitchState(PunchState.Idle);
        }
    }

    public void OnExit()
    {
        patrolPosition++;
        if (patrolPosition >= PDSetting.PatrolRange.Length)
        {
            patrolPosition = 0;
        }
    }
}
