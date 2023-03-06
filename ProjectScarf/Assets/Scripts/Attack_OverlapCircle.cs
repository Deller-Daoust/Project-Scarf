using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_OverlapCircle : MonoBehaviour
{
    private GameObject player;
    private bool playerColliding;

    void Start()
    {
        player = GameObject.FindWithTag("PlayerHitbox");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            playerColliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            playerColliding = false;
        }
    }


    public Collider2D IsOverlapping()
    {
        if (playerColliding)
        {
            return player.GetComponent<Collider2D>();
        }
        else
        {
            return null;
        }
    }
}
