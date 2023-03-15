using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPartol : ShootBase
{
    private ShootFSM ShootFSM;
    private ShootSetting ShootSetting;
    // Start is called before the first frame update
    private int patrolPosition;
    public ShootPartol(ShootFSM shootFSM)
    {
        this.ShootFSM = shootFSM;
        this.ShootSetting = shootFSM.shootSetting;
    }
    public void OnEnter()
    {
        ShootSetting.animator.Play("ShootPartol");
    }

    public void OnUpdate()
    {
        ShootFSM.ChangeDirection(ShootSetting.PatrolRange[patrolPosition]);
        ShootFSM.transform.position = Vector2.MoveTowards(ShootFSM.transform.position, ShootSetting.PatrolRange[patrolPosition].position, ShootSetting.moveSpeed * Time.deltaTime);

        if (ShootSetting.BeAttacked == true)
        {
            ShootFSM.SwitchState(ShootState.UnderAttack);
        }
        if (ShootSetting.FindPlayer != null && ShootSetting.FindPlayer.position.x >= ShootSetting.ChaseRange[0].position.x && ShootSetting.FindPlayer.position.x <= ShootSetting.ChaseRange[1].position.x)
        {
            ShootFSM.SwitchState(ShootState.Chase);
        }
        if (Vector2.Distance(ShootFSM.transform.position, ShootSetting.PatrolRange[patrolPosition].position) < 0.1f)
        {
            ShootFSM.SwitchState(ShootState.Idle);
        }
    }

    public void OnExit()
    {
        patrolPosition++;
        if (patrolPosition >= ShootSetting.PatrolRange.Length)
        {
            patrolPosition = 0;
        }
    }
}
