using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootIdle : ShootBase
{
    private ShootFSM ShootFSM;
    private ShootSetting ShootSetting;
    // Start is called before the first frame update
    private float timer;
    public ShootIdle(ShootFSM shootFSM)
    {
        this.ShootFSM = shootFSM;
        this.ShootSetting = shootFSM.shootSetting;
    }
    public void OnEnter()
    {
        ShootSetting.animator.Play("ShootIdle");
    }

     public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (ShootSetting.BeAttacked == true)
        {
            ShootFSM.SwitchState(ShootState.UnderAttack);
        }
        if (ShootSetting.FindPlayer != null && ShootSetting.FindPlayer.position.x >= ShootSetting.ChaseRange[0].position.x && ShootSetting.FindPlayer.position.x <= ShootSetting.ChaseRange[1].position.x)
        {
            ShootFSM.SwitchState(ShootState.Chase);
        }
        if (timer >= ShootSetting.idleTime)
        {
            ShootFSM.SwitchState(ShootState.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;
    }
}
