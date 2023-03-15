using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Pickup : MonoBehaviour
{
    public int health = 1;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag.Equals("Player"))
        {
            if (player.gameObject.GetComponent<Combat_System>().hp < 6)
            {
                player.gameObject.GetComponent<Combat_System>().hp++;
                Destroy(gameObject);
            }
        }
    }
}
