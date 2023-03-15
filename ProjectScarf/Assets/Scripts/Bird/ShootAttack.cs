using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : ShootBase
{
    private ShootFSM ShootFSM;
    private ShootSetting ShootSetting;
    // Start is called before the first frame update
    private AnimatorStateInfo stateInfo;
    public ShootAttack(ShootFSM shootFSM)
    {
        this.ShootFSM = shootFSM;
        this.ShootSetting = shootFSM.shootSetting;
    }
    public void OnEnter()
    {
        ShootSetting.animator.Play("ShootAttack");
    }

    public void OnUpdate()
    {
        stateInfo = ShootSetting.animator.GetCurrentAnimatorStateInfo(0);
        if (ShootSetting.BeAttacked == true)
        {
            ShootFSM.SwitchState(ShootState.UnderAttack);
        }
        if (stateInfo.normalizedTime >= 0.95f)
        {
            ShootFSM.SwitchState(ShootState.Chase);
        }
    }

    public void OnExit()
    {

    }
}
