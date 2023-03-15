using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_SliceAttack : MonoBehaviour
{
    private Hit_Player hp;
    private GameObject sam;
    public float dSelf = 0.1f;
    public bool canParry = true;
    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<Hit_Player>();
        if (canParry)
        {
            sam = GameObject.Find("Samurai");
        }
        Invoke("DestroySelf",dSelf);
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.transform.parent.GetComponent<Player_Movement>().rolling <= 0)
        {
            hp.HitPlayer();
            if (!player.transform.parent.GetComponent<Player_Movement>().didGetHit && canParry)
            {
                sam.GetComponent<Samurai_Behaviour>().parryCounter--;
            }
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
