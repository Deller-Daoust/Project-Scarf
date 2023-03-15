using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_SpinAttack : MonoBehaviour
{
    Hit_Player hp;
    bool canHit = true;
    private GameObject sam;
    void Start()
    {
        hp = GetComponent<Hit_Player>();
        sam = GameObject.Find("Samurai");
    }

    void OnTriggerStay2D(Collider2D player)
    {
        if (player.transform.parent.GetComponent<Player_Movement>().rolling <= 0 && canHit)
        {
            hp.HitPlayer();
            if (!player.transform.parent.GetComponent<Player_Movement>().didGetHit)
            {
                sam.GetComponent<Samurai_Behaviour>().parryCounter--;
            }
            canHit = false;
            Invoke("CanHit",0.4f);
        }
    }

    void CanHit()
    {
        canHit = true;
    }
}
