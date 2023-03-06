using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle_Follow : MonoBehaviour
{
    private GameObject player;
    private Vector2 targetPos;
    [SerializeField] private float retSpeed = 15f;
    private bool following = true, playerInBounds;
    [SerializeField] private float followTime, armTime, triggerTime, blinkRate, polishRate;
    private SpriteRenderer sprite;
    public ParticleSystem gunshot;
    public AudioSource aSource;
    public AudioClip reticleSound, shotSound;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        InvokeRepeating("Flash", followTime + armTime, blinkRate);
        sprite.color = new Color(1,1,1,0);
        transform.localScale = new Vector3(4f, 4f, 4f);
        StartCoroutine(JaydensIdea());
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            targetPos = new Vector2(player.transform.position.x, player.transform.position.y + 0.7f);
            transform.position = Vector2.Lerp(transform.position, targetPos, retSpeed * Time.deltaTime);
        }
        sprite.color = Color.Lerp(sprite.color, new Color(1, 1, 1, 1), polishRate * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2f, 2f, 2f), polishRate * Time.deltaTime);
    }
    void Flash()
    {
        sprite.enabled = !sprite.enabled;
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

    IEnumerator JaydensIdea()
    {
        yield return new WaitForSeconds(followTime);
        following = false;
        yield return new WaitForSeconds(armTime);
        aSource.PlayOneShot(reticleSound);
        yield return new WaitForSeconds(triggerTime);
        gunshot.Play();
        if (playerInBounds && player.GetComponent<Player_Movement>().rolling <= 0)
        {
            GetComponent<Hit_Player>().HitPlayer();
        }
        aSource.PlayOneShot(shotSound);
        CancelInvoke();
        sprite.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
