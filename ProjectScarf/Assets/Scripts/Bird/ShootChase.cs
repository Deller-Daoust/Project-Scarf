using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootChase : ShootBase
{
    private ShootFSM ShootFSM;
    private ShootSetting ShootSetting;
    // Start is called before the first frame update

    public ShootChase(ShootFSM shootFSM)
    {
        this.ShootFSM = shootFSM;
        this.ShootSetting = shootFSM.shootSetting;
    }
    public void OnEnter()
    {
       ShootSetting.animator.Play("ShootChase");
    }

    public void OnUpdate()
    {
        ShootFSM.ChangeDirection(ShootSetting.FindPlayer);
        if (ShootSetting.BeAttacked == true)
        {
            ShootFSM.SwitchState(ShootState.UnderAttack);
        }
        if (ShootSetting.FindPlayer)
        {
            ShootFSM.transform.position = Vector2.MoveTowards(ShootFSM.transform.position, ShootSetting.FindPlayer.position, ShootSetting.ChaseSpeed * Time.deltaTime);
        }
        if (ShootSetting.FindPlayer == null || ShootFSM.transform.position.x < ShootSetting.ChaseRange[0].position.x || ShootFSM.transform.position.x > ShootSetting.ChaseRange[1].position.x)
        {
            ShootFSM.SwitchState(ShootState.Idle);
        }
        if (Physics2D.OverlapCircle(ShootSetting.AttackRange.position, ShootSetting.AttackNumber, ShootSetting.LayerMask))
        {
            ShootFSM.SwitchState(ShootState.Attack);
            ShootFSM.source.PlayOneShot(ShootFSM.load);
        }
    }

    public void OnExit()
    {

    }
}
