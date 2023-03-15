using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDPatrol : EBase
{
    private EFSM EFSM;
    private EDSetting EDSetting;
    private int patrolPosition;

    public EDPatrol(EFSM eFSM)
    {
        this.EFSM = eFSM;
        this.EDSetting = eFSM.EDSetting;
    }

    public void OnEnter()
    {
        EDSetting.Times = EDSetting.StartTime;
        EDSetting.animator.Play("EDPatrol");
        EDSetting.NextPosition.position = EFSM.GetRandomPos();
    }

    public void OnUpdate()
    {
        EFSM.ChangeDirection(EDSetting.PatrolRange[patrolPosition]);
        if (EDSetting.Times<-1)
        {
            EDSetting.Times = EDSetting.StartTime;
        }
        EFSM.transform.position = Vector2.MoveTowards(EFSM.transform.position,EDSetting. NextPosition.position, EDSetting.moveSpeed * Time.deltaTime);
        if (Vector2.Distance(EFSM.transform.position,EDSetting.NextPosition.position)<0.1f)
        {
            if (EDSetting.Times<=0)
            {
               EDSetting. NextPosition.position = EFSM.GetRandomPos();
                EDSetting.Times = EDSetting.StartTime;
            }
            else
            {
                EDSetting.Times -= Time.deltaTime;
            }
        }

        if (EDSetting.BeAttacked == true)
        {
            EFSM.SwitchState(ExplodeState.UnderAttack);
        }
        if (EDSetting.FindPlayer != null && EDSetting.FindPlayer.position.x >= EDSetting.ChaseRange[0].position.x && EDSetting.FindPlayer.position.x <= EDSetting.ChaseRange[1].position.x)
        {
            EFSM.SwitchState(ExplodeState.Chase);
        }
        if (Vector2.Distance(EFSM.transform.position, EDSetting.PatrolRange[patrolPosition].position) < 0.1f)
        {
            EFSM.SwitchState(ExplodeState.Idle);
        }
    }

    public void OnExit()
    {
        patrolPosition++;
        if (patrolPosition >= EDSetting.PatrolRange.Length)
        {
            patrolPosition = 0;
        }
    }
}
