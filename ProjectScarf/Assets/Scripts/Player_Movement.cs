using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // This really should be called Player_Controller but it's way too late to change it now lol

    /*private float topSpeed;
    private float speedDiff;
    private float accelRate;*/

    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float frictionValue;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedPow;
    [SerializeField] private GameObject corpse;

    public bool canMove;

    [SerializeField] private float jumpForce;
    private float coyoteTime = 0.15f;
    private float coyoteCounter;
    private bool isJumping;
    private bool jumpBuffer;

    public float gravityScale;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isOnGround;

    //camshit
    [SerializeField] private bool camFollow = true;

    private float moveSpeed;
    public Vector2 moveInput;

    public bool facingRight = false;

    [HideInInspector] public Rigidbody2D body;
    private SpriteRenderer sprite;
    public Animator animator;

    //movement bools
    [SerializeField] private float rollCooldown;
    private bool canRoll = true;
    public int rolling = 0;
    public float playerDir;
    public float rollPower;

    //audio bools
    public AudioSource musicSource, sfxSource;
    [SerializeField] private AudioSource stepSource;
    [SerializeField] private AudioClip jumpSound, parrySuccess, rollSound;

    //white sprite stuff
    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public GameObject Attack;

    private Combat_System combat;
    public bool canInput;

    private Vector3 targetPos;
    public float camSpeed = 15f;

    [SerializeField] private Collider2D[] ledgeDetection;
 

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        combat = GetComponent<Combat_System>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");

        canMove = true;
        canInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (camFollow)
        {
            targetPos = new Vector3(gameObject.transform.position.x/* + (moveInput.x)*/, gameObject.transform.position.y + 1f, gameObject.transform.position.z - 1f);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, camSpeed * Time.deltaTime);
        }

        if(canMove == true)
        {
            if (canInput) moveInput.x = Input.GetAxisRaw("Horizontal");

            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsRolling", false);

            if(moveInput.x > 0 || moveInput.x < 0)
            {
                if(animator.GetBool("IsJumping") == false && isOnGround)
                {
                    animator.SetBool("IsRunning", true);
                }
            }
            if (animator.GetBool("IsFalling") == true || moveInput.x == 0)
            {
                animator.SetBool("IsRunning", false);
            }

            if(moveInput.x < 0 && !facingRight)
            {
                Flip();
            }
            if(moveInput.x > 0 && facingRight)
            {
                Flip();
            }

            if(Input.GetButtonDown("Jump"))
            {
                jumpBuffer = true;
                Invoke("JumpBuffering", 0.2f);
            }

            if(jumpBuffer && coyoteCounter > 0f && canInput)
            {
                Jump();
                coyoteCounter = 0f;
                sfxSource.pitch = Random.Range(0.95f,1.05f);
                sfxSource.PlayOneShot(jumpSound);
            }

            
        }
        else
        {
            moveInput.x = 0;

            animator.SetBool("IsRunning", false);
            animator.SetBool("IsFalling", false);
        }

        if (facingRight)
            {
                playerDir = -1f;
            }
            else
            {
                playerDir = 1f;
            }

            //roll
            if(Input.GetKeyDown(KeyCode.LeftShift) && canRoll && canInput)
            {
                combat.CancelAttacks();
                combat.gunShot = false;
                combat.canScarf = true;
                combat.canAttack = true;
                if (combat.activeDude != null)
                {
                    Destroy(combat.activeDude);
                }
                canMove = true;
                animator.SetBool("IsRolling", true);

                body.velocity = new Vector2(body.velocity.x + (playerDir * rollPower), body.velocity.y);
                sfxSource.PlayOneShot(rollSound);

                /*if(animator.GetBool("IsRunning") == false)
                {
                    //body.AddForce(rollPower * Vector2.right, ForceMode2D.Impulse);
                    body.velocity = new Vector2(rollPower * playerDir);
                }
                else
                {
                    
                    //body.AddForce(rollPower * -Vector2.right * playerDir, ForceMode2D.Impulse);
                    body.velocity = new Vector2(rollPower * playerDir);
                }*/

                canRoll = false;
                StartCoroutine(cooldownRoll());
                rolling = 25;
            }

        if(Input.GetButtonUp("Jump") && body.velocity.y > 0f)
        {
            body.AddForce(Vector2.down * body.velocity.y * (1 - 0.4f), ForceMode2D.Impulse);

            coyoteCounter = 0f;
        }

        if(isJumping && body.velocity.y < 0f)
        {
            isJumping = false;
        }
        if(body.velocity.y < 0f && !isOnGround)
        {
            animator.SetBool("IsFalling", true);
            body.gravityScale = gravityScale * 2f;
        }
        else
        {
            animator.SetBool("IsFalling", false);
            body.gravityScale = gravityScale;
        }

        //isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isOnGround = Physics2D.OverlapBox(new Vector2(groundCheck.position.x, groundCheck.position.y + 0.12f), new Vector2(0.4f, 0.4f), 0f, groundLayer);
        if(isOnGround)
        {
            coyoteCounter = coyoteTime;
            //make stepping audio play when moving
            if (moveInput.x != 0f && !stepSource.isPlaying)
            {
                stepSource.Play();
            }
            //stop sound if holding both keys
            if (moveInput.x == 0f)
            {
                stepSource.Stop();
            }
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
            stepSource.Stop();
        }

        //stop sound if let go of key
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            stepSource.Stop();
        }

        if (body.velocity.y > 0f)
        {
            coyoteCounter = 0f;
        }

        if(isOnGround == true)
        {
            animator.SetBool("IsFalling", false);
        }
    }

    private void FixedUpdate()
    {
        rolling--;
        if (rolling > 0)
        {
            decceleration = 0.5f;
        }
        else
        {
            decceleration = 16f;
        }
        // The topSpeed is the speed we're aiming to be at the apex of the run, which is the value of the horizontal input multiplied by the max speed.
        float topSpeed = moveInput.x * maxSpeed;
        // Then we smooth it out with Mathf.Lerp, taking in the velocity of the rigidbody at that time and the top speed, and a lerp value (which in this case is 1).
        topSpeed = Mathf.Lerp(body.velocity.x, topSpeed, 1); 

        float speedDiff = topSpeed - body.velocity.x; // The difference in speed between the current velocity of the body, and the top speed we're aiming for.

        float accelRate = (Mathf.Abs(topSpeed) > 0.01f) ? acceleration : decceleration; // The rate of acceleraion/decceleration  //spell acceleration and deceleration right silly

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, speedPow) * Mathf.Sign(speedDiff);

        body.AddForce(movement * Vector2.right);

        //what da heck is this comment yer shaboinkies pritty please :D!!
        if(isOnGround && Mathf.Abs(moveInput.x) < 0.01f)
        {
            float friction = Mathf.Min(Mathf.Abs(body.velocity.x), Mathf.Abs(frictionValue));
            friction *= Mathf.Sign(body.velocity.x);

            body.AddForce(Vector2.right * -friction, ForceMode2D.Impulse);
        }

        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -30f, 9999f));
    } 

    public void Jump()
    {
        animator.SetBool("IsRolling", false);
        animator.Play("Player_Jump", -1, 0f);
        rolling = 0;
        isJumping = true;
        coyoteCounter = 0f;
        //body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        body.velocity = new Vector2(body.velocity.x, jumpForce);
    }

    private void JumpBuffering()
    {
        jumpBuffer = false;
    }

    private void Flip()
    {
        Vector3 currentDirect = gameObject.transform.localScale;
        currentDirect.x *= -1;
        gameObject.transform.localScale = currentDirect;

        facingRight = !facingRight;
    }

    private IEnumerator cooldownRoll()
    {
        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
        flashSprite();
    }

    private void flashSprite() 
    {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
        StartCoroutine(unwhiteSprite());
    }

    private IEnumerator unwhiteSprite()
    {
        yield return new WaitForSeconds(0.06f);
        myRenderer.material.shader = shaderSpritesDefault;
        myRenderer.color = Color.white;
    }

    public IEnumerator GetHit(float _dir, int _dmg = 1)
    {
        if (!GetComponent<Combat_System>().parrying)
        {
            combat.hp -= _dmg;
            if (combat.hp <= 0) 
            {
                Die();
            }
            else
            {
                StartCoroutine(SetIFrames());
                sfxSource.PlayOneShot(combat.hitSound);
                gravityScale = 1.7f;
                canMove = false;
                flashSprite();
                myRenderer.color = Color.red;
                Time.timeScale = 0f;
                Invoker.InvokeDelayed(ResumeTime, 0.2f);
                body.velocity = new Vector2(15f * _dir, 7f);
                decceleration = 5f;
                yield return new WaitForSeconds(0.3f);
                decceleration = 16f;
                canMove = true;
            }
        }
        else
        {
            Invoker.InvokeDelayed(ResumeTime, 0.15f);
            sfxSource.PlayOneShot(parrySuccess);
            GetComponent<Combat_System>().parrying = false;
            GetComponent<Combat_System>().GetBullet();
            gravityScale = 1.7f;
            canMove = true;
            GetComponent<Combat_System>().canParry = true;
            Time.timeScale = 0f;
        }
    }

    IEnumerator SetIFrames()
    {
        GetComponent<Combat_System>().hitbox.SetActive(false);
        InvokeRepeating("FlashSprite",0.01f, 0.1f);
        yield return new WaitForSeconds(1f);
        CancelInvoke("FlashSprite");
        sprite.color = new Color(1, 1, 1, 1);
        GetComponent<Combat_System>().hitbox.SetActive(true);
    }

    void FlashSprite()
    {
        if (sprite.color == new Color(1, 1, 1, 1))
        {
            sprite.color = new Color(1, 1, 1, 0);
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
    }

    void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    // Ledge detection.
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7 && collision.IsTouching(ledgeDetection[0]))
        {

            if(collision.IsTouching(ledgeDetection[1]))
            {
                return;
            }
            else
            {
                if(isOnGround == false)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                }
            }
        }
    }

    void Die()
    {
        Instantiate(corpse, transform.position,Quaternion.identity);
        gameObject.SetActive(false);
    }
}