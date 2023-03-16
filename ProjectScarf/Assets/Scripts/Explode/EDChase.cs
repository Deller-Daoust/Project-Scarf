using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDChase : EBase
{
    private EFSM EFSM;
    private EDSetting EDSetting;

    public EDChase(EFSM eFSM)
    {
        this.EFSM = eFSM;
        this.EDSetting = eFSM.EDSetting;
    }

    public void OnEnter()
    {
        EDSetting.animator.Play("EDChase");
    }

    public void OnUpdate()
    {
        EFSM.gameObject.GetComponent<AudioSource>().pitch = 1.7f;
        EFSM.ChangeDirection(EDSetting.FindPlayer);
        EFSM.transform.localScale = new Vector2(-EFSM.transform.localScale.x, 1f);
        if (EDSetting.BeAttacked == true)
        {
            EFSM.SwitchState(ExplodeState.UnderAttack);
        }
        if (EDSetting.FindPlayer)
        {
            EFSM.transform.position = Vector2.MoveTowards(EFSM.transform.position, new Vector2(EDSetting.FindPlayer.position.x, EDSetting.FindPlayer.position.y + 0.7f), EDSetting.ChaseSpeed * Time.deltaTime);
        }
        if (EDSetting.FindPlayer == null || EFSM.transform.position.x < EDSetting.ChaseRange[0].position.x || EFSM.transform.position.x > EDSetting.ChaseRange[1].position.x)
        {
            EFSM.SwitchState(ExplodeState.Idle);
        }
        if (Physics2D.OverlapCircle(EDSetting.AttackRange.position, EDSetting.AttackNumber, EDSetting.LayerMask))
        {
            EDSetting.canAttack = true;
            EFSM.SwitchState(ExplodeState.Attack);
        }
    }

    public void OnExit()
    {

    }
}
