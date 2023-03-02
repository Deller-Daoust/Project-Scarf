using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty_Behaviour : MonoBehaviour
{
    private string state = "idle";
    private bool canStartShooting = true;
    public float moveCooldown;

    public float maxSpeed, acceleration, speedPow, decceleration, frictionValue;
    private float friction, topSpeed;
    public bool isOnGround = true, useLandmines, phase2;
    public float spawnSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private int randomInt, oldRandomInt;
    private GameObject player;


    [SerializeField] private GameObject pistolReticle, mgunBullet, railgun, rocket, landmine;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("TestStates", 0f, moveCooldown);
        Invoke("SetPhase2",30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (state.Equals("machinegun"))
        {
            if (canStartShooting)
            {
                StartCoroutine(MachineGun());
                canStartShooting = false;
            }
        }
        if (state.Equals("pistol"))
        {
            if (canStartShooting)
            {
                StartCoroutine(PistolShots());
                canStartShooting = false;
            }
        }
        if (state.Equals("railgun"))
        {
            if (canStartShooting)
            {
                StartCoroutine(Railgun());
                canStartShooting = false;
            }
        }
        if (state.Equals("rocket"))
        {
            if (canStartShooting)
            {
                Rocket();
                canStartShooting = false;
            }
        }
        if (phase2)
        {
            if (moveCooldown == 5f)
            {
                moveCooldown = 3f;
                spawnSpeed = 1.5f;
                CancelInvoke("TestStates");
                InvokeRepeating("TestStates", 0f, moveCooldown);
            }
            spawnSpeed = 1.5f;
            player.GetComponent<Player_Movement>().musicSource.pitch = 1.25f;
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 1.15f;
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

    void MakeBullet()
    {
        Instantiate(mgunBullet, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.identity);
    }

    void Rocket()
    {
        Instantiate(rocket, new Vector2(transform.position.x, transform.position.y + 1.75f), Quaternion.identity);
    }

    IEnumerator MachineGun()
    {
        InvokeRepeating("MakeBullet",0f,0.05f / spawnSpeed);
        yield return new WaitForSeconds(moveCooldown - 0.1f);
        CancelInvoke("MakeBullet");
        state = "idle";
    }

    IEnumerator Railgun()
    {
        Instantiate(railgun, new Vector2(0f,0.5f),Quaternion.identity);
        yield return new WaitForSeconds(0.8f / spawnSpeed);
        Instantiate(railgun, new Vector2(0f,0.5f),Quaternion.identity);
        yield return new WaitForSeconds(0.8f / spawnSpeed);
        Instantiate(railgun, new Vector2(0f,0.5f),Quaternion.identity);
        if (phase2)
        {
            yield return new WaitForSeconds(0.8f / spawnSpeed);
            Instantiate(railgun, new Vector2(0f,0.5f),Quaternion.identity);
        }
    }

    IEnumerator PistolShots()
    {
        Instantiate(pistolReticle, new Vector2(0f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f / spawnSpeed);
        Instantiate(pistolReticle, new Vector2(0f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f / spawnSpeed);
        Instantiate(pistolReticle, new Vector2(0f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f / spawnSpeed);
        Instantiate(pistolReticle, new Vector2(0f, 0f), Quaternion.identity);
        if (phase2)
        {
            yield return new WaitForSeconds(0.5f / spawnSpeed);
            Instantiate(pistolReticle, new Vector2(0f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(0.5f / spawnSpeed);
            Instantiate(pistolReticle, new Vector2(0f, 0f), Quaternion.identity);
        }
    }

    void TestStates()
    {
        int oldRandomInt = randomInt;
        randomInt = Random.Range(1, 6);
        while (randomInt == oldRandomInt)
        {
            randomInt = Random.Range(1, 6);
        }

        switch (randomInt)
        {
            case 1:
                SwitchState("pistol");
                break;
            case 2:
                SwitchState("machinegun");
                break;
            case 3:
                SwitchState("railgun");
                break;
            case 4:
                SwitchState("rocket");
                break;
            case 5:
                SwapSides();
                break;
        }
    }
    
    void SwitchState(string _state)
    {
        state = _state;
        canStartShooting = true;
    }

    void GoIdle()
    {
        SwitchState("idle");
    }

    IEnumerator Run(Vector2 _moveInput, float _time)
    {
        moveInput = _moveInput;
        yield return new WaitForSeconds(_time/3);
        if (useLandmines)
        {
            Instantiate(landmine,transform.position,Quaternion.identity);
        }
        yield return new WaitForSeconds(_time/3);
        if (useLandmines)
        {
            Instantiate(landmine,transform.position,Quaternion.identity);
        }
        yield return new WaitForSeconds(_time/3);
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        moveInput = Vector2.zero;
        gameObject.layer = LayerMask.NameToLayer("Boss");
    }

    void SwapSides()
    {
        gameObject.layer = LayerMask.NameToLayer("BossHookable");
        if (transform.position.x < 0)
        {
            StartCoroutine(Run(Vector2.right, 1.4f));
        }
        else
        {
            StartCoroutine(Run(Vector2.left, 1.4f));
        }
    }

    void SetPhase2()
    {
        phase2 = true;
    }
}
