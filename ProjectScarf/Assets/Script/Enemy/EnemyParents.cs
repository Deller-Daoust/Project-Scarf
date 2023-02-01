using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParents : MonoBehaviour
{
    public int enemyHealth;
    public int enemyDamage;
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (enemyHealth <=0)
        {
            Destroy(gameObject);
        }
    }

    public void DamageTake(int Damage)
    {
        enemyHealth -= Damage;
    }
}
