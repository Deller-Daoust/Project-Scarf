using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Behaviour : MonoBehaviour
{
    private GameObject player;
    public float speed, rotationModifier, rocketSpeed, sinSpeed;
    private Rigidbody2D rb;
    private float sinThing;
    private bool rotating = true;
    public ParticleSystem firePS, ps;
    private AudioSource source;
    public AudioClip explosion;
    private float prevSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StopRotating());
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && rotating)
        {
            rotationModifier = 180 + (Mathf.Sin(sinThing) * 30);
            Vector3 vectorToTarget = new Vector3(player.transform.position.x, player.transform.position.y + 0.75f, player.transform.position.z) - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        }
    }

    void FixedUpdate()
    {
        rocketSpeed += 6f / 500f;
        //speed += 6f / 500f;
        sinThing += sinSpeed;
        Vector2 v1 = transform.rotation * Vector3.left * rocketSpeed;
        rb.velocity = v1;
        if (GetComponent<SpriteRenderer>().enabled == false)
        {
            rb.velocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && collider.gameObject.transform.parent.GetComponent<Player_Movement>().rolling <= 0 && GetComponent<SpriteRenderer>().enabled)
        {
            if (!collider.gameObject.transform.parent.GetComponent<Combat_System>().parrying)
            {
                StartCoroutine(Die());
            }
            else
            {
                transform.localRotation *= Quaternion.Euler(0, 0, 180);
                prevSpeed = speed;
                speed = 0f;
                Invoke("ReturnSpeed",0.5f);
            }
        }
    }

    void ReturnSpeed()
    {
        speed = prevSpeed;
    }

    IEnumerator StopRotating()
    {
        yield return new WaitForSeconds(10f);
        rotating = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    IEnumerator Die()
    {
        ps.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        source.PlayOneShot(explosion);
        GetComponent<Hit_Player>().HitPlayer();
        firePS.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
