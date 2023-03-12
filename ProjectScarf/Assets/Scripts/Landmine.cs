using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem explodePS;
    private AudioSource source;
    public float timer;
    public AudioClip explosion;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 11f);
        StartCoroutine(Die2());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("PlayerHitbox") && GetComponent<SpriteRenderer>().enabled && rb.velocity.y == 0f)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        GetComponent<Hit_Player>().HitPlayer();
        explodePS.Play();
        source.PlayOneShot(explosion);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator Die2()
    {
        yield return new WaitForSeconds(timer);
        if (GetComponent<SpriteRenderer>().enabled)
        {
        explodePS.Play();
        source.PlayOneShot(explosion);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        }
    }
}
