using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun_Bullet_Behaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public ParticleSystem ps;
    private GameObject boss;
    private float bossDir;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boss = GameObject.Find("Bounty Hunter");
        bool tempDir = boss.GetComponent<SpriteRenderer>().flipX;
        if (tempDir)
        {
            bossDir = 1f;
        }
        else
        {
            bossDir = -1f;
        }
        rb.velocity = new Vector2(-15f * bossDir, Random.Range(-1f, 1f));
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(Die());
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player") && collider.gameObject.GetComponent<Player_Movement>().rolling <= 0)
        {
            StartCoroutine(Die2());
        }
    }

    IEnumerator Die()
    {
        ps.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator Die2()
    {
        ps.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        //GetComponent<Hit_Player>().HitPlayer();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
