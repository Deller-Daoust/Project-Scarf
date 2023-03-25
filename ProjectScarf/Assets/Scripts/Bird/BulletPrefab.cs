using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    Hit_Player hp;
    GameObject playur;
    void Start()
    {
        playur = GameObject.FindWithTag("Player");
        hp = GetComponent<Hit_Player>();
        Invoke("DestroySelf", 3f);
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag.Equals("PlayerHitbox"))
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
                    gameObject.layer = LayerMask.NameToLayer("PlayerAttacks");
                }
            }
        }
        else
        {
            if (player.GetComponent<ShootFSM>() != null)
            {
                player.GetComponent<ShootFSM>().shootSetting.BeAttacked = true;
            }
            if (player.GetComponent<FSM>() != null)
            {
                player.GetComponent<FSM>().enemySetting.BeAttacked = true;
            }
            if (player.GetComponent<HP_Handler>() != null)
            {
                player.GetComponent<HP_Handler>().health -= 5;
                if (player.GetComponent<HP_Handler>().health <= 0)
                {
                    playur.GetComponent<Combat_System>().ComboStuff(player.tag);
                }
                    playur.GetComponent<Player_Movement>().sfxSource.PlayOneShot(playur.GetComponent<Combat_System>().hitSound);
            }
            if (player.GetComponent<Flash_Functions>() != null)
            {
                player.GetComponent<Flash_Functions>().flashSprite(Color.red);
            }
            Destroy(gameObject);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(10f * playur.GetComponent<Player_Movement>().playerDir, 0f);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
