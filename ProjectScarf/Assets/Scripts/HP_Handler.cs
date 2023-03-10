using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Handler : MonoBehaviour
{
    public int health = 10;
    public GameObject corpse;
    public bool isStunned;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (corpse != null)
            {
                Instantiate(corpse, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    void Unstun()
    {
        isStunned = false;
    }
}
