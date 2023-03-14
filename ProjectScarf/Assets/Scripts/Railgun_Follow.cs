using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun_Follow : MonoBehaviour
{
    private float followSpeed, speedGain = 1f;
    private Vector2 targetPos;
    private GameObject player;
    private AudioSource source;
    private bool following = true;
    [SerializeField] private AudioClip chargeSound, laserSound;
    public GameObject laser;
    public ParticleSystem laserPS, poundPS;
    private bool playerInBounds;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        source = GetComponent<AudioSource>();
        source.PlayOneShot(chargeSound);
        StartCoroutine(Explode());
        Invoke("StopFollowing", 0.75f);
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector2(player.transform.position.x, 0.5f);
        if (following)
        {
            transform.position = Vector2.Lerp(transform.position,targetPos,followSpeed * Time.deltaTime);
        }
        laser.transform.localScale = Vector2.Lerp(laser.transform.localScale, Vector3.one, followSpeed * Time.deltaTime);
        
        GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, new Color(1f, 0f, 0f, 0.2f), Time.deltaTime * 5f);
        laser.GetComponent<SpriteRenderer>().color = Color.Lerp(laser.GetComponent<SpriteRenderer>().color, new Color(1f, 0f, 0f, 1f), Time.deltaTime * 5f);
    }

    void FixedUpdate()
    {
        followSpeed += speedGain/50f;
        speedGain *= 1.14f;
    }

    void OnTriggerEnter2D(Collider2D collidr)
    {
        if (collidr.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
        {
            playerInBounds = true;
        }
    }
    void OnTriggerExit2D(Collider2D collidr)
    {
        if (collidr.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
        {
            playerInBounds = false;
        }
    }

    void StopFollowing()
    {
        following = false;
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.8f);
        source.Stop();
        source.PlayOneShot(laserSound);
        poundPS.Play();
        laserPS.Play();
        if (playerInBounds && player.GetComponent<Player_Movement>().rolling <= 0)
        {
            GetComponent<Hit_Player>().HitPlayer();
        }
        GetComponent<SpriteRenderer>().enabled = false;
        laser.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
