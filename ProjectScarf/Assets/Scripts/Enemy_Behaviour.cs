using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Rigidbody2D body;

    public void OnHit(float damage, Vector2 knockback)
    {
        body.AddForce(knockback, ForceMode2D.Impulse);
    }

    public void OnHit(float damage)
    {

    }
}
