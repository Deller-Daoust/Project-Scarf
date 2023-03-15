using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    Hit_Player hp;
    void Start()
    {
        hp = GetComponent<Hit_Player>();
        Invoke("DestroySelf", 3f);
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.transform.parent.GetComponent<Player_Movement>().rolling <= 0)
        {
            hp.HitPlayer();
            if (player.transform.parent.GetComponent<Player_Movement>().didGetHit)
            {
                Destroy(gameObject);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.x,GetComponent<Rigidbody2D>().velocity.y);
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
