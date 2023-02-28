using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    /*private float topSpeed;
    private float speedDiff;
    private float accelRate;*/

    [SerializeField] private Pause_Menu pauseMenu;

    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float frictionValue;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedPow;

    [SerializeField] private float jumpForce;
    private float coyoteTime = 0.10f;
    private float coyoteCounter;
    private bool isJumping;

    private float gravityScale = 1.7f;

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

    public GameObject Attack;
    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z - 1);

        moveInput.x = Input.GetAxisRaw("Horizontal");

        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsRolling", false);


        if(pauseMenu.gameIsPaused == false)
        {
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
        }

        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if(isOnGround)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && coyoteCounter > 0f)
        {
            Jump();
        }

        if(Input.GetButtonUp("Jump") && body.velocity.y > 0f)
        {
            body.AddForce(Vector2.down * body.velocity.y * (1 - 0.4f), ForceMode2D.Impulse);

            coyoteCounter = 0f;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("IsRolling", true);
            body.AddForce(Vector2.right * body.velocity.x * 2, ForceMode2D.Impulse);
        }

        if(isJumping && body.velocity.y < 0f)
        {
            isJumping = false;
        }

        if(body.velocity.y < 0f)
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

    private void FixedUpdate()
    {
        // The topSpeed is the speed we're aiming to be at the apex of the run, which is the value of the horizontal input multiplied by the max speed.
        float topSpeed = moveInput.x * maxSpeed;
        // Then we smooth it out with Mathf.Lerp, taking in the velocity of the rigidbody at that time and the top speed, and a lerp value (which in this case is 1).
        topSpeed = Mathf.Lerp(body.velocity.x, topSpeed, 1); 

        float speedDiff = topSpeed - body.velocity.x; // The difference in speed between the current velocity of the body, and the top speed we're aiming for.

        float accelRate = (Mathf.Abs(topSpeed) > 0.01f) ? acceleration : decceleration; // The rate of acceleraion/decceleration 

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, speedPow) * Mathf.Sign(speedDiff);

        body.AddForce(movement * Vector2.right);

        if(isOnGround && Mathf.Abs(moveInput.x) < 0.01f)
        {
            float friction = Mathf.Min(Mathf.Abs(body.velocity.x), Mathf.Abs(frictionValue));
            friction *= Mathf.Sign(body.velocity.x);

            body.AddForce(Vector2.right * -friction, ForceMode2D.Impulse);
        }
    } 

    private void Jump()
    {
        isJumping = true;

        if(body.velocity.y < 0)
        {
            body.AddForce(Vector2.up * jumpForce * 1.3f, ForceMode2D.Impulse);
        }
        else
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        Vector3 currentDirect = gameObject.transform.localScale;
        currentDirect.x *= -1;
        gameObject.transform.localScale = currentDirect;

        facingRight = !facingRight;
    }

    
}
