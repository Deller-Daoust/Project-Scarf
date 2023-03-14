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
    private string state = "chaseslice";
    private float dir = -1f;

    private Animator anim;
    private AudioSource source;
    public bool isOnGround = true;
    private bool isNear = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
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
                state = "slicing";
            }
        }
    }

    private void FixedUpdate()
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

    void ChasePlayer()
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

    void GetNear()
    {
        isNear = false;
        if (transform.position.x < player.transform.position.x + 1.5f && transform.position.x > player.transform.position.x - 1.5f)
        {
            isNear = true;
        }
    }
}
