using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    /*private float topSpeed;
    private float speedDiff;
    private float accelRate;*/

    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float frictionValue;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedPow;

    [SerializeField] private float jumpForce;
    private float coyoteTime = 0.15f;
    private float coyoteCounter;
    private bool isJumping;

    [SerializeField] private float gravityScale;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isOnGround;

    private float moveSpeed;
    private Vector2 moveInput;

    public bool facingRight = false;

    [HideInInspector] public Rigidbody2D body;
    private SpriteRenderer sprite;
    public Animator animator;

    //movement bools
    [SerializeField] private float rollCooldown, savedVelocity;
    private bool canRoll = true;
    private int rolling = 0;

    //audio bools
    [SerializeField] private AudioSource stepSource, sfxSource;
    [SerializeField] private AudioClip jumpSound;

    //white sprite stuff
    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public GameObject Attack;


    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    // Update is called once per frame
    void Update()
    {
        rolling--;
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z - 1);

        moveInput.x = Input.GetAxisRaw("Horizontal");

        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsRolling", false);

        if(moveInput.x > 0 || moveInput.x < 0)
        {
            if(animator.GetBool("IsJumping") == false)
            {
                animator.SetBool("IsRunning", true);
            }
        }
        else
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

        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if(isOnGround)
        {
            coyoteCounter = coyoteTime;
            //make stepping audio play when moving
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !stepSource.isPlaying && !(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
            {
                stepSource.Play();
            }
            //stop sound if holding both keys
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
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

        if(Input.GetButtonDown("Jump") && coyoteCounter > 0f)
        {
            Jump();
            sfxSource.pitch = Random.Range(0.95f,1.05f);
            sfxSource.PlayOneShot(jumpSound);
        }

        if(Input.GetButtonUp("Jump") && body.velocity.y > 0f)
        {
            body.AddForce(Vector2.down * body.velocity.y * (1 - 0.4f), ForceMode2D.Impulse);

            coyoteCounter = 0f;
        }


        //roll
        if(Input.GetKeyDown(KeyCode.LeftShift) && canRoll)
        {
            animator.SetBool("IsRolling", true);
            body.AddForce(Vector2.right * body.velocity.x * 2, ForceMode2D.Impulse);
            canRoll = false;
            savedVelocity = body.velocity.x;
            StartCoroutine(cooldownRoll());
            rolling = 40;
        }

        if(isJumping && body.velocity.y < 0f)
        {
            isJumping = false;
        }

        
    }

    private void FixedUpdate()
    {
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

        if(body.velocity.y < -0.0001f)
        {
            animator.SetBool("IsFalling", true);
            body.gravityScale = gravityScale * 2f;
        }
        else
        {
            animator.SetBool("IsFalling", false);
            body.gravityScale = gravityScale;
        }
        
    } 

    private void Jump()
    {
        animator.SetBool("IsRolling", false);
        animator.Play("Player_Jump", -1, 0f);
        rolling = 0;
        isJumping = true;
        coyoteCounter = 0f;
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
}