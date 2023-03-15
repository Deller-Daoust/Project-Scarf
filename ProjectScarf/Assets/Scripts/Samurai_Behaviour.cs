using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Behaviour : MonoBehaviour
{
    public float maxSpeed, acceleration, speedPow, decceleration, frictionValue;
    private float friction, topSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private GameObject player;
    public string state = "idle";
    private float dir = -1f;
    private SpriteRenderer sprite;
    public GameObject warning, sliceAttack, windAttack, spinAttack;
    public LayerMask groundLayer;
    public float signAffector = 0.7f;
    public int parryCounter = 5;

    private Animator anim;
    private AudioSource source;
    public bool isOnGround = true;
    private bool isNear = false;
    public bool canMove = true;
    public bool phase2 = false;

    private HP_Handler hp;

    public int chaseCount = 3;

    private Coroutine coSpin, coSlash, coWind, coStomp, coStun, coStates;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<HP_Handler>();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        coStates = StartCoroutine(StartStateCycle());
    }

    // Update is called once per frame
    void Update()
    {
        if (hp.health <= 0 && !phase2)
        {
            hp.health = hp.maxHealth;
            phase2 = true;
            if (coStates != null)
            {
                StopCoroutine(coStates);
            }
            CancelStates();
            player.GetComponent<Player_Movement>().musicSource.pitch = 1.15f;
            if (coStun != null)
            {
                StopCoroutine(coStun);
                coStun = null;
            }
            coStates = StartCoroutine(StartStateCycle(1f));
        }
        if (phase2)
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 1.15f;
            }
        }
        GetNear();
        moveInput = Vector2.zero;
        if (!isNear)
        {
            if (state.Equals("chaseslice"))
            {
                ChasePlayer();
            }
        }
        else
        {
            if (state.Equals("chaseslice"))
            {
                coSlash = StartCoroutine(Slice());
            }
        }

        if (state.Equals("spin"))
        {
            transform.localScale = new Vector2(Mathf.Sign(-rb.velocity.x), 1f);
            spinAttack.SetActive(true);
        }
        else
        {
            spinAttack.SetActive(false);
        }

        if (state.Equals("stunned"))
        {
            gameObject.layer = LayerMask.NameToLayer("BossHittable");
            canMove = true;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Boss");
        }

        if (parryCounter <= 0)
        {
            coStun = StartCoroutine(Stun());
        }

        ShouldLook();

    }

    IEnumerator Stun()
    {
        parryCounter = 5;
        SwitchState("stunned");
        CancelStates();
        if (coStates != null)
        {
            StopCoroutine(coStates);
            coStates = null;
        }
        yield return new WaitForSeconds(4f);
        SwitchState("idle");
        coStates = StartCoroutine(StartStateCycle(2f));
        coStun = null;
    }

    IEnumerator StartStateCycle(float _delay = 0f)
    {
        Debug.Log("called statecycle");
        yield return new WaitForSeconds(_delay);
        Debug.Log("chase slice switched");
        SwitchState("chaseslice");
        chaseCount = 3;
        yield return new WaitUntil(IsIdle);
        yield return new WaitForSeconds(1f);
        coSpin = StartCoroutine(Spin());
        yield return new WaitUntil(IsIdle);
        yield return new WaitForSeconds(2f);
        coWind = StartCoroutine(WindSlash());
        yield return new WaitUntil(IsIdle);
        yield return new WaitForSeconds(2f);
        coStun = StartCoroutine(Stun());
    }

    IEnumerator WindSlash()
    {
        SwitchState("windslashing");
        yield return new WaitForSeconds(0.5f);
        Warning();
        yield return new WaitForSeconds(0.5f);
        GameObject summonedWind = Instantiate(windAttack, new Vector2(transform.position.x,transform.position.y + 0.9f), Quaternion.identity);
        summonedWind.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * -12f, 0f);
        yield return new WaitForSeconds(1f);
        SwitchState("idle");
    }

    bool IsIdle()
    {
        if (state.Equals("idle"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        if (state.Equals("spin"))
        {
            float signThing;

            if (Mathf.Sign(player.transform.position.x - transform.position.x) == 1)
            {
                signThing = signAffector;
            }
            else
            {
                signThing = -signAffector;
            }
            rb.velocity = new Vector2(rb.velocity.x + signThing, rb.velocity.y);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 13f);
        }

        isOnGround = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + 0.12f), new Vector2(0.4f, 0.4f), 0f, groundLayer);
        
        if (isOnGround)
        {
            decceleration = 16f;
        }
        else
        {
            decceleration = 4f;
        }

        if (rb.velocity.y > 0f)
        {
            rb.gravityScale = 1.7f;
        }
        else
        {
            rb.gravityScale = 3.4f;
        }
        if (canMove)
        {
            // The topSpeed is the speed we're aiming to be at the apex of the run, which is the value of the horizontal input multiplied by the max speed.
            float topSpeed = moveInput.x * maxSpeed;
            // Then we smooth it out with Mathf.Lerp, taking in the velocity of the rigidbody at that time and the top speed, and a lerp value (which in this case is 1).
            topSpeed = Mathf.Lerp(rb.velocity.x, topSpeed, 1); 

            float speedDiff = topSpeed - rb.velocity.x; // The difference in speed between the current velocity of the body, and the top speed we're aiming for.

            float accelRate = (Mathf.Abs(topSpeed) > 0.01f) ? acceleration : decceleration; // The rate of acceleraion/decceleration  //spell acceleration and deceleration right silly

            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, speedPow) * Mathf.Sign(speedDiff);

            rb.AddForce(movement * Vector2.right);

            //what da heck is this comment yer shaboinkies pritty please :D!!
            if(isOnGround && Mathf.Abs(moveInput.x) < 0.01f)
            {
                float friction = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionValue));
                friction *= Mathf.Sign(rb.velocity.x);

                rb.AddForce(Vector2.right * -friction, ForceMode2D.Impulse);
            }
        }
    } 

    private void CancelStates()
    {
        Debug.Log("states cancelled");
        if (coSlash != null)
        {
            StopCoroutine(coSlash);
            coSlash = null;
        }
        if (coWind != null)
        {
            StopCoroutine(coWind);
            coWind = null;
        }
        if (coStomp != null)
        {
            StopCoroutine(coStomp);
            coStomp = null;
        }
        if (coSpin != null)
        {
            StopCoroutine(coSpin);
            coSpin = null;
        }
    }

    private void SwitchState(string _state)
    {
        state = _state;
    }

    IEnumerator Spin()
    {
        Warning();
        SwitchState("prespin");
        yield return new WaitForSeconds(1f);
        canMove = false;
        SwitchState("spin");
        yield return new WaitForSeconds(1.6f);
        Warning();
        yield return new WaitForSeconds(0.4f);
        rb.velocity = new Vector2(rb.velocity.x, 15f);
        yield return new WaitForSeconds(1.6f);
        Warning();
        yield return new WaitForSeconds(0.4f);
        rb.velocity = new Vector2(rb.velocity.x, 15f);
        yield return new WaitForSeconds(2f);
        SwitchState("idle");
        canMove = true;

    }

    IEnumerator Slice()
    {
        SwitchState("slicing");
        LookAtPlayer();
        chaseCount--;
        Warning();
        float playerSide = Mathf.Sign(player.transform.position.x - transform.position.x);
        yield return new WaitForSeconds(0.2f);
        canMove = false;
        rb.velocity = new Vector2(rb.velocity.x, ((player.transform.position.y + (player.GetComponent<Rigidbody2D>().velocity.y * 0.2f)) - transform.position.y) * 4f);
        if (playerSide == Mathf.Sign((player.transform.position.x + (player.GetComponent<Rigidbody2D>().velocity.x * 0.2f)) - transform.position.x))
        {
            rb.velocity = new Vector2(((player.transform.position.x + (player.GetComponent<Rigidbody2D>().velocity.x * 0.2f)) - transform.position.x) * 4.5f, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        Instantiate(sliceAttack, this.transform);
        yield return new WaitForSeconds(0.5f);
        if (chaseCount > 0)
        {
            SwitchState("chaseslice");
        }
        else
        {
            SwitchState("idle");
        }
    }

    void ChasePlayer()
    {
        if (isOnGround)
        {
            if (transform.position.x > player.transform.position.x)
            {
                moveInput = Vector2.left;
            }
            else
            {
                moveInput = Vector2.right;
            }
        }
    }

    void ShouldLook()
    {
        switch (state)
        {
            case ("chaseslice"):
                LookAtPlayer();
                break;
            case ("idle"):
                LookAtPlayer();
                break;
        }
    }

    void LookAtPlayer()
    {
        if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    void Warning()
    {
        Instantiate(warning, this.transform);
    }

    void GetNear()
    {
        isNear = false;
        if (transform.position.x < player.transform.position.x + 3.5f && transform.position.x > player.transform.position.x - 3.5f)
        {
            isNear = true;
        }
    }
}
