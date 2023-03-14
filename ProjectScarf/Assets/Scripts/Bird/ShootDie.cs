using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDie : ShootBase
{
    private ShootFSM ShootFSM;
    private ShootSetting ShootSetting;
    // Start is called before the first frame update

    public ShootDie(ShootFSM shootFSM)
    {
        this.ShootFSM = shootFSM;
        this.ShootSetting = shootFSM.shootSetting;
    }
    public void OnEnter()
    {
        ShootSetting.animator.Play("ShootDie");

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
