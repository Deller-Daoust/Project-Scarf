using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Handler : MonoBehaviour
{
    public int health = 10, maxHealth = 10;
    public GameObject corpse;
    public bool isStunned, dieAtZero = true;

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (health <= 0)
        {
            if (corpse != null)
            {
                Instantiate(corpse, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            if (dieAtZero)
            {
                Destroy(gameObject);
            }
        }
    }

    void Unstun()
    {
        isStunned = false;
    }
}
