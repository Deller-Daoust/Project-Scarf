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
    public float stunTime;
    private int hp = 3;
    private GameObject bh;
    public AudioSource chaseSource;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        bh = GameObject.Find("Bounty Hunter");
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StopRotating());
        source = GetComponent<AudioSource>();
        Invoke("MoreSpeed",0.7f);
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
        if (transform.rotation.eulerAngles.z >= 90 && transform.rotation.eulerAngles.z <= 270)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipY = false;
        }
        if (hp <= 0 && GetComponent<SpriteRenderer>().enabled)
        {
            player = bh;
            StopCoroutine(StopRotating());
            gameObject.layer = LayerMask.NameToLayer("PlayerAttacks");
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
        if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && collider.gameObject.transform.parent.GetComponent<Player_Movement>().rolling <= 0 && GetComponent<SpriteRenderer>().enabled && speed > 0f)
        {
            if (!collider.gameObject.transform.parent.GetComponent<Combat_System>().parrying)
            {
                StartCoroutine(Die());
            }
            else
            {
                player.GetComponent<Player_Movement>().ParrySuccess();
                transform.localRotation *= Quaternion.Euler(0, 0, 180);
                hp--;
                if (hp > 0)
                {
                    prevSpeed = speed;
                    speed = 0f;
                    Invoke("ReturnSpeed",stunTime);
                }
            }
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss") && GetComponent<SpriteRenderer>().enabled && speed > 0f && !bh.GetComponent<Bounty_Behaviour>().state.Equals("stunned"))
        {
            Bounty_Behaviour bb = bh.GetComponent<Bounty_Behaviour>();
            StartCoroutine(Die2());
            bb.anim.Play("BH_Stun");
            bb.rb.velocity = new Vector2(3f * -bb.dir, 5f);
            bb.state = "stunned";
            bb.StartRecover();
            bb.StopCoroutine(bb.coStates);
            bb.CancelInvoke("MakeBullet");
            bb.wallSource.Play();
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
        chaseSource.Stop();
        source.PlayOneShot(explosion);
        GetComponent<Hit_Player>().HitPlayer();
        firePS.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator Die2()
    {
        ps.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        chaseSource.Stop();
        source.PlayOneShot(explosion);
        firePS.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void MoreSpeed()
    {
        rocketSpeed = 3f;
        speed = 6f;
        chaseSource.volume = 1f;
    }
}
