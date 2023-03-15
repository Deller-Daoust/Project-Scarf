using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootUnderAttack : ShootBase
{
    private ShootFSM ShootFSM;
    private ShootSetting ShootSetting;
    // Start is called before the first frame update
    private AnimatorStateInfo stateInfo;
    public ShootUnderAttack(ShootFSM shootFSM)
    {
        this.ShootFSM = shootFSM;
        this.ShootSetting = shootFSM.shootSetting;
    }
    public void OnEnter()
    {
        ShootSetting.animator.Play("ShootUnderattack");
    }

    public void OnUpdate()
    {
        stateInfo = ShootSetting.animator.GetCurrentAnimatorStateInfo(0);
        if (ShootSetting.health <= 0)
        {
            ShootFSM.SwitchState(ShootState.Death);
        }
        if (stateInfo.normalizedTime >= 0.95f)
        {
            ShootSetting.FindPlayer = GameObject.FindWithTag("Player").transform;
            ShootFSM.SwitchState(ShootState.Chase);
        }
    }

    public void OnExit()
    {
        ShootSetting.getHit = false;
    }
}
