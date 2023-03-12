using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bounty_Behaviour : MonoBehaviour
{
    public string state = "idle";
    private bool canStartShooting = true;
    public float moveCooldown;

    public float maxSpeed, acceleration, speedPow, decceleration, frictionValue;
    private float friction, topSpeed;
    public bool isOnGround = true, useLandmines = true, phase2;
    public float spawnSpeed;
    private Vector2 moveInput;
    public Rigidbody2D rb;
    private int randomInt, oldRandomInt;
    private GameObject player;
    private bool canMove;
    public float stunFactor = 1f;

    public Animator anim;
    private AudioSource source;
    public AudioSource wallSource;
    public AudioClip railgunSound;
    public float dir = -1f;
    public Light2D light;
    public GameObject hookWarning;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext, shaderSpritesDefault;
    private HP_Handler hp;

    public Coroutine coStates, coRecover;


    [SerializeField] private GameObject pistolReticle, mgunBullet, railgun, rocket, landmine;
    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<HP_Handler>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        coStates = StartCoroutine(BetterStates(2.66f));
        anim.Play("BH_Laugh");
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.activeSelf)
        {
            state = "idle";
            CancelInvoke();
        }

        if (hp.health <= 0 && !phase2)
        {
            phase2 = true;
            hp.health = hp.maxHealth;
        }

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
                StartCoroutine(Rocket());
                canStartShooting = false;
            }
        }
        if (phase2)
        {
            if (moveCooldown == 5f)
            {
                moveCooldown = 3f;
                spawnSpeed = 1.5f;
                GoIdle();
                anim.Play("BH_Transition");
                StopCoroutine(coRecover);
                StopCoroutine(coStates);
                StartCoroutine(BetterStates(2f));
                myRenderer.flipX = !myRenderer.flipX;
                useLandmines = true;
                spawnSpeed = 1.5f;
                player.GetComponent<Player_Movement>().musicSource.pitch = 1.15f;
            }
            if (NoAnimsPlaying())
            {
                anim.Play("BH_Idle2");
            }
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 1.15f;
            }
        }
        else
        {
            if (NoAnimsPlaying())
            {
                anim.Play("BH_Idle");
            }
        }

        if (state.Equals("stunned"))
        {
            gameObject.layer = LayerMask.NameToLayer("BossHittable");
            canMove = false;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Boss");
            canMove = true;
        }
        if (myRenderer.flipX)
        {
            dir = -1f;
        }
        else
        {
            dir = 1f;
        }
    }

    private void FixedUpdate()
    {
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

    public void StartRecover()
    {
        coRecover = StartCoroutine(Recover());
    }

    void OnTriggerEnter2D(Collider2D wall)
    {
        if (wall.gameObject.layer == LayerMask.NameToLayer("Ground") && !state.Equals("stunned"))
        {
            anim.Play("BH_Stun");
            rb.velocity = new Vector2(3f * -dir, 5f);
            state = "stunned";
            StartRecover();
            wallSource.Play();
        }
    }

    public IEnumerator BetterStates(float _delay = 0f)
    {
        Debug.Log("coroutine called");
        yield return new WaitForSeconds(_delay);
        Debug.Log("move 1");
        RandomState();
        yield return new WaitForSeconds(moveCooldown);
        Debug.Log("move 2");
        RandomState();
        yield return new WaitForSeconds(moveCooldown);
        Debug.Log("move 3");
        SwitchState("rocket");
        yield return new WaitForSeconds(moveCooldown);
        Debug.Log("move 4");
        RandomState();
        yield return new WaitForSeconds(moveCooldown);
        Debug.Log("move 5");
        RandomState();
        yield return new WaitForSeconds(moveCooldown);
        Debug.Log("run");
        SwapSides();
    }

    void RandomState()
    {
        int oldRandomInt = randomInt;
        randomInt = Random.Range(1, 4);
        while (randomInt == oldRandomInt)
        {
            randomInt = Random.Range(1, 4);
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
        }
    }

    public IEnumerator Recover()
    {
        MakeSpriteWhite();
        moveInput = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
        yield return new WaitForSeconds((0.5f * stunFactor) - 0.1f);
        MakeSpriteNormal();
        yield return new WaitForSeconds(0.5f * stunFactor);
        MakeSpriteWhite();
        yield return new WaitForSeconds(0.5f * stunFactor);
        MakeSpriteNormal();
        yield return new WaitForSeconds(0.45f * stunFactor);
        MakeSpriteWhite();
        yield return new WaitForSeconds(0.4f * stunFactor);
        MakeSpriteNormal();
        yield return new WaitForSeconds(0.35f * stunFactor);
        MakeSpriteWhite();
        yield return new WaitForSeconds(0.3f * stunFactor);
        MakeSpriteNormal();
        yield return new WaitForSeconds(0.25f * stunFactor);
        MakeSpriteWhite();
        yield return new WaitForSeconds(0.2f * stunFactor);
        MakeSpriteNormal();
        yield return new WaitForSeconds(0.15f * stunFactor);
        MakeSpriteWhite();
        yield return new WaitForSeconds(0.1f * stunFactor);
        MakeSpriteNormal();
        yield return new WaitForSeconds(0.1f * stunFactor);
        MakeSpriteWhite();
        yield return new WaitForSeconds(0.1f * stunFactor);
        MakeSpriteNormal();
        GoIdle();
        Debug.Log("went idle");
        if (transform.position.x > 0)
        {
            myRenderer.flipX = true;
        }
        else
        {
            myRenderer.flipX = false;
        }
        Debug.Log("flipped sprite");
        StartCoroutine(BetterStates(2f));
        Debug.Log("started coroutine");
    }

    void MakeSpriteWhite()
    {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
    }

    void MakeSpriteNormal()
    {
        myRenderer.color = Color.white;
        myRenderer.material.shader = shaderSpritesDefault;
    }

    void MakeBullet()
    {
        Instantiate(mgunBullet, new Vector2(transform.position.x + (2f * dir), transform.position.y + 1.1f), Quaternion.identity);
    }

    IEnumerator Rocket()
    {
        anim.Play("BH_RocketLauncher");
        yield return new WaitForSeconds(0.5f);
        if (dir == -1f)
        {
            Instantiate(rocket, new Vector2(transform.position.x + (1.2f * dir), transform.position.y + 1.9f), Quaternion.identity);
        }
        else
        {
            Instantiate(rocket, new Vector2(transform.position.x + (1.2f * dir), transform.position.y + 1.9f), Quaternion.Euler (0f, 180f, 0f));
        }
        GoIdle();
    }

    IEnumerator MachineGun()
    {
        anim.Play("BH_Minigun");
        light.color = new Color(1, 1, 100/255, 1);
        hookWarning.SetActive(true);
        yield return new WaitForSeconds(1.4f * 1.3f);
        InvokeRepeating("MakeBullet",0f,0.05f / spawnSpeed);
        yield return new WaitForSeconds((1.3f * 1.3f) * (moveCooldown/5f));
        CancelInvoke("MakeBullet");
        light.color = new Color(1, 1, 1, 1);
        hookWarning.SetActive(false);
        GoIdle();
    }

    IEnumerator Railgun()
    {
        source.PlayOneShot(railgunSound);
        Instantiate(railgun, new Vector2(player.transform.position.x,0.5f),Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        anim.Play("BH_Railgun");
        yield return new WaitForSeconds(0.5f / spawnSpeed);
        Instantiate(railgun, new Vector2(player.transform.position.x,0.5f),Quaternion.identity);
        yield return new WaitForSeconds(0.8f / spawnSpeed);
        Instantiate(railgun, new Vector2(player.transform.position.x,0.5f),Quaternion.identity);
        if (phase2)
        {
            yield return new WaitForSeconds(0.8f / spawnSpeed);
            Instantiate(railgun, new Vector2(player.transform.position.x,0.5f),Quaternion.identity);
        }
        GoIdle();
    }

    IEnumerator PistolShots()
    {
        anim.Play("BH_Pistol");
        //yield return new WaitForSeconds(0.3f);
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
        GoIdle();
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
        state = "running";
        moveInput = _moveInput;
        yield return new WaitForSeconds(0.5f);
        if (useLandmines)
        {
            Instantiate(landmine,transform.position,Quaternion.identity);
        }
        yield return new WaitForSeconds(0.5f);
        if (useLandmines)
        {
            Instantiate(landmine,transform.position,Quaternion.identity);
        }
        //yield return new WaitForSeconds(1f);
        //GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        //dir = -dir;
        //moveInput = Vector2.zero;
        //GoIdle();
        //gameObject.layer = LayerMask.NameToLayer("Boss");
    }

    void SwapSides()
    {
        gameObject.layer = LayerMask.NameToLayer("BossHittable");
        anim.Play("BH_Slide");
        if (transform.position.x < Camera.main.transform.position.x)
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

    bool NoAnimsPlaying()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
