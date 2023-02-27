using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Combat_System combat;
    public float timer;
    public int damage;
   public PolygonCollider2D colliderBox;

    public bool attackEnemy;
    // Start is called before the first frame update
    void Start()
    {
        colliderBox = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        attack();
    }
    
    void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {

            colliderBox.enabled = true;
            StartCoroutine(HideHitbox());
        }

    }

    IEnumerator HideHitbox()
    {
        yield return new WaitForSeconds(timer);
        colliderBox.enabled=false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<FSM>().DamageTake(damage);
            attackEnemy = true;
        }
      
        
    }
}
