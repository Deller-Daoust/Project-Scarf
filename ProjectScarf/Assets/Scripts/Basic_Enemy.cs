using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy : MonoBehaviour
{
    private float damage = 1;
    private float knockbackForce = 50f;

    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if(damageable != null)
        {
            Vector2 direction = (col.transform.position - transform.position).normalized;

            Vector2 knockback = direction * knockbackForce;

            damageable.OnHit(damage, knockback);
        }
    }
}
