using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_System : MonoBehaviour
{
    private Player_Movement playerMove;

    //public GameObject scarf;
    
    private Vector3 targetScale;
    private Vector3 baseScale;


    public int hp = 6;

    private Vector3 startPos;
    private Vector3 targetPos;

    public int totalHealthLost;

    private Vector3 mousePos;
    private Vector3 mouseRot;
    //private float xMin = -1f, xMax = 1f;
    //private float yMin = -1f, yMax = 1f;
    private float xPos, yPos;

    public bool scarfOut = false;
    public bool gunShot = false;
    public bool parrying = false;
    public bool canParry = true;
    public bool canAttack = true;
    public bool slashing = false;

    private bool scarfBuffering;
    private bool swordBuffering;
    private bool gunBuffering;
    [SerializeField] private float attackBuffer;

    //public IEnumerator coScarf, coSword, coGun, coParry;
    private Coroutine coSword, coGun, coScarf, coReturn, coScarfOut;

    public GameObject hitbox, parryIndicator, hookScarf;

    //private float LerpTime = 1f;

    //public Coroutine _scarfOut;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask buttonLayer;

    [Header("Sounds")]
    public AudioClip parrySound, chompSound, critSound, gunSound, swordSound, hitSound;

    [Header("Snake chomp")]
    public GameObject chompDude;
    public GameObject activeDude;

    [Header("Gun")]
    [SerializeField] private Transform gun;
    private float gunRange = 0.5f;
    public bool hasBullet = false;
    public GameObject bulletIndicator;

    [SerializeField] private Transform sword;
    private float swordRange = 0.85f;

    [SerializeField] private Transform scarf;
    public Vector2 scarfRange = new Vector2(9f, 3f);

    [SerializeField] private Transform wallCheck;
    private Vector2 wallRange = new Vector2(9f, 1.5f);

    public bool canScarf = true;

    private Vector2 playerCenter;

    // Start is called before the first frame update
    void Start()
    {
        targetScale = new Vector3(5, 0.5f, 0);

        /*baseScale = scarf.transform.localScale;

        startPos = scarf.transform.localPosition;*/

        playerMove = GetComponent<Player_Movement>();
        //coScarf = Scarf();
        //coSword = SwordAttack();
        //coGun = GunBlast();
    }

    // Update is called once per frame
    void Update()
    {
        hp = Mathf.Clamp(hp, 0, 6);

        if (parrying)
        {
            hitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.15f, 0.4f);
        }
        else
        {
            hitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.1004214f,0.3406209f);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            scarfBuffering = true;
            CancelInvoke("ScarfBuffer");
            Invoke("ScarfBuffer", attackBuffer);
        }


        if(scarfBuffering && gunShot == false)
        {
            if(canScarf && playerMove.canInput)
            {
                if (coScarf != null)
                {
                    StopCoroutine(coScarf);
                    coScarf = null;
                    Debug.Log("ended scarf");
                }
                if (coScarfOut != null)
                {
                    StopCoroutine(coScarfOut);
                    coScarfOut = null;
                    Debug.Log("ended scarfout");
                }
                //coScarf = Scarf();
                if (coScarf == null)
                {
                    coScarf = StartCoroutine(Scarf());
                    scarfBuffering = false;
                    Debug.Log("started scarf");
                }
                coScarfOut = StartCoroutine(ScarfOut());
            }
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            if(canParry && playerMove.canInput)
            {
                StartCoroutine(Parry());
            }
        }

        if(xPos >= 0)
        {
            xPos = 1;
        }
        else if(xPos < 0)
        {
            xPos = -1;
        }

        scarf.transform.Rotate(mouseRot * Time.deltaTime * 1.0f);

        targetPos = new Vector3(xPos, startPos.y, 0);
        //targetPos = new Vector3(xPos, yPos, 0);

        playerMove.animator.SetBool("IsMeleeing", false);
        playerMove.animator.SetBool("IsShooting", false);

        if(Input.GetMouseButtonDown(0))
        {
            swordBuffering = true;
            CancelInvoke("SwordBuffer");
            Invoke("SwordBuffer", attackBuffer);
        }

        if(swordBuffering && scarfOut == false && gunShot == false && canAttack && playerMove.canInput)
        {
            //coSword = SwordAttack();
            if (coSword == null)
            {
                coSword = StartCoroutine(SwordAttack());
                swordBuffering = false;
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            gunBuffering = true;
            CancelInvoke("GunBuffer");
            Invoke("GunBuffer", attackBuffer);
        }

        if(gunBuffering && scarfOut == false && hasBullet && playerMove.canInput && !gunShot && !slashing)
        {
            gunShot = true;
            //coGun = GunBlast();
            if (coGun == null)
            {
                coGun = StartCoroutine(GunBlast());
                gunBuffering = false;
            }
        }

        parryIndicator.SetActive(parrying);
    }

    #region Attacks

    public IEnumerator Scarf()
    {
        if (coReturn != null)
        {
            StopCoroutine(coReturn);
            coReturn = null;
        }

        canScarf = false;

        RaycastHit2D wall = Physics2D.Raycast(wallCheck.position, Vector2.right, Mathf.Infinity, groundLayer);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(scarf.position, scarfRange, 0, enemyLayers);

        if(enemies.Length > 0) 
        {
            GameObject closestEnemy = enemies[0].gameObject;
            float dist = Vector3.Distance(transform.position, enemies[0].transform.position);
            for(int i = 0; i < enemies.Length; i++)
            {
                float tempDist = Vector3.Distance(transform.position, enemies[i].transform.position);
                if(tempDist < dist) 
                {
                    closestEnemy = enemies[i].gameObject;
                }
            }

            if(closestEnemy != null)
            {
                if(wall.collider && playerMove.camFollow)
                {
                    float wallDist = wall.distance;
                    float enemyDist = Vector3.Distance(transform.position, closestEnemy.transform.position);

                    if(wallDist > enemyDist)
                    {
                        StartCoroutine(ScarfTele(closestEnemy));
                    }
                }
                else
                {
                    StartCoroutine(ScarfTele(closestEnemy));
                }
            }
        }

        yield return new WaitForSeconds(0.8f); // Cooldown

        canScarf = true;
        coScarf = null;
    }

    void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    IEnumerator ScarfTele(GameObject closestEnemy)
    {
        activeDude = Instantiate(chompDude, closestEnemy.transform.position, Quaternion.identity);
        activeDude.GetComponent<Snake_Chomp>().target = closestEnemy;
        yield return new WaitForSeconds(0.38f/1.38f);
        GetComponent<Player_Movement>().sfxSource.PlayOneShot(chompSound);
        if (closestEnemy.GetComponent<HP_Handler>() != null && closestEnemy.GetComponent<RandomScream>() != null)
        {
            if (closestEnemy.GetComponent<HP_Handler>().health <= 10 && hasBullet)
            {
                int i = Random.Range(0, 50);
                if (i == 0)
                {
                    closestEnemy.GetComponent<RandomScream>().Scream();
                }
            }
        }
        yield return new WaitForSeconds(0.08f/1.38f);
        Invoker.InvokeDelayed(ResumeTime,0.05f);
        canScarf = true;
        Time.timeScale = 0f;
        closestEnemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        activeDude.GetComponent<Snake_Chomp>().chompPS.Play();
        if (closestEnemy.GetComponent<FSM>() != null)
        {
            FSM fsm = closestEnemy.GetComponent<FSM>();
            fsm.SwitchState(StateType.Idle);
            fsm.enemySetting.BeAttacked = true;
        }
        
        if (closestEnemy.GetComponent<ShootFSM>() != null)
        {
            ShootFSM fsm = closestEnemy.GetComponent<ShootFSM>();
            fsm.SwitchState(ShootState.Idle);
            fsm.shootSetting.BeAttacked = true;
        }
        if (closestEnemy.GetComponent<HP_Handler>() != null)
        {
            closestEnemy.GetComponent<HP_Handler>().isStunned = true;
            if (closestEnemy.GetComponent<HP_Handler>().IsInvoking("Unstun"))
            {
                closestEnemy.GetComponent<HP_Handler>().CancelInvoke("Unstun");
            }
            closestEnemy.GetComponent<HP_Handler>().Invoke("Unstun", 1f);
        }
        yield return new WaitForSeconds(0.01f);
        playerMove.SetIFrames(0.2f, false);
        playerMove.transform.position = new Vector2 (closestEnemy.transform.position.x - (playerMove.playerDir * 1.2f), closestEnemy.transform.position.y);
        closestEnemy = null;
    }

    public IEnumerator ScarfOut()
    {
        playerMove.canMove = false;

        Debug.Log("scarfout");
        playerMove.animator.Play("Player_Scarf",-1, 0f);
        yield return new WaitForSeconds(0.6f);
        playerMove.canMove = true;
    }

    public IEnumerator SwordAttack()
    {
        if (coReturn != null)
        {
            StopCoroutine(coReturn);
            coReturn = null;
        }
        
        slashing = true;
        GetComponent<Player_Movement>().canMove = false;
        GetComponent<Player_Movement>().rolling = 0;
        playerMove.animator.SetBool("IsMeleeing", true);
        canAttack = false;
        
        yield return new WaitForSeconds(0.07f);
        HitEnemies("sword");
        GetComponent<Player_Movement>().sfxSource.PlayOneShot(swordSound);
        yield return new WaitForSeconds(0.12f);
        HitEnemies("sword2");
        GetComponent<Player_Movement>().sfxSource.PlayOneShot(swordSound);
        slashing = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Player_Movement>().canMove = true;
        yield return new WaitForSeconds(0.1f);
        canAttack = true;
        coSword = null;
    }

    void HitEnemies(string _type)
    {
        int _damage = 2;
        float _force = 3f, _stun = 0.05f;
        switch (_type)
        {
            case "sword":
                _damage = 2;
                _force = 3f;
                _stun = 0.05f;
                break;
            case "sword2":
                _damage = 2;
                _force = 15f;
                _stun = 0.08f;
                break;
            case "gun":
                _damage = 5;
                _force = 8f;
                _stun = 0.12f;
                break;
        }
        Collider2D[] enemies = Physics2D.OverlapCircleAll(sword.position, swordRange, enemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            if (enemy.GetComponent<FSM>() != null)
            {
                switch (_type)
                {
                    case "gun":
                    case "sword2":
                        enemy.GetComponent<FSM>().enemySetting.BeAttacked = true;
                        break;
                }
            }

            if (enemy.GetComponent<ShootFSM>() != null)
            {
                switch (_type)
                {
                    case "gun":
                    case "sword":
                    case "sword2":
                        enemy.GetComponent<ShootFSM>().shootSetting.BeAttacked = true;
                        break;
                }
            }

            if (enemy.GetComponent<HP_Handler>() != null)
            {
                if (_type.Equals("gun"))
                {
                    if (enemy.GetComponent<HP_Handler>().isStunned)
                    {
                        _damage = 10;
                        _force = 15f;
                        _stun = 0.25f;
                        enemy.GetComponent<HP_Handler>().isStunned = false;
                        GetComponent<Player_Movement>().sfxSource.PlayOneShot(critSound);
                        if (enemy.name.Equals("Crit Dummy"))
                        {
                            _damage = 100000;
                        }
                    }
                }
                if (enemy.gameObject.layer != LayerMask.NameToLayer("Boss"))
                {
                    enemy.GetComponent<HP_Handler>().health -= _damage;
                    if (playerMove.combo > 0)
                    {
                        playerMove.comboTimer += ((float)_damage) / 2f;
                    }
                }
                else
                {
                    enemy.GetComponent<HP_Handler>().health -= Mathf.Max(1, (int)Mathf.Floor((float)_damage/4f));
                }
                if (enemy.GetComponent<HP_Handler>().health <= 0 && enemy.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    ComboStuff(enemy.tag);
                }
                GetComponent<Player_Movement>().sfxSource.PlayOneShot(hitSound);
            }
            
            if (enemy.GetComponent<Flash_Functions>() != null)
            {
                enemy.GetComponent<Flash_Functions>().flashSprite(Color.red);
            }
            if (enemies[0] != null)
            {
                if (_stun <= 0.08f)
                {
                    GetBullet();
                }
            }
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(_force * GetComponent<Player_Movement>().playerDir, 0f);
            }
            Time.timeScale = 0f;
            Invoker.InvokeDelayed(ResumeTime, _stun/2);
        }
    }

    void HitButton()
    {
        Collider2D button = Physics2D.OverlapCircle(sword.position, swordRange, buttonLayer);
        if (button != null)
        {
            if (button.GetComponent<Gun_Button>().shots > 0)
            {
                button.GetComponent<Gun_Button>().shots--;
            }
        }
    }

    public void ComboStuff(string _enemy)
    {
        playerMove.comboTimer = 10f;
        if (_enemy.Equals("Ant"))
        {
            playerMove.comboBase += 100;
            playerMove.combo++;
        }
        if (_enemy.Equals("Drone"))
        {
            playerMove.comboBase += 125;
            playerMove.combo++;
        }
        if (_enemy.Equals("Bird"))
        {
            playerMove.comboBase += 150;
            playerMove.combo++;
        }
        if (_enemy.Equals("Ox"))
        {
            playerMove.comboBase += 200;
            playerMove.combo++;
        }
    }

    public IEnumerator GunBlast()
    {
        if (coReturn != null)
        {
            StopCoroutine(coReturn);
            coReturn = null;
        }

        GetComponent<Player_Movement>().canMove = false;
        GetComponent<Player_Movement>().rolling = 0;
        playerMove.animator.SetBool("IsShooting", true);
        yield return new WaitForSeconds(0.35f/1.2f);
        hasBullet = false;
        HitEnemies("gun");
        HitButton();
        GetComponent<Player_Movement>().sfxSource.PlayOneShot(gunSound);
        yield return new WaitForSeconds(0.1f/1.2f);
        GetComponent<Player_Movement>().canMove = true;
        gunShot = false;
        coGun = null;
    } 

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(sword.position, swordRange);
        Gizmos.DrawWireSphere(gun.position, gunRange);
        Gizmos.DrawWireCube(scarf.position, scarfRange);
    }

    private void ScarfBuffer()
    {
        scarfBuffering = false;
    }

    private void SwordBuffer()
    {
        swordBuffering = false;
    }

    private void GunBuffer()
    {
        gunBuffering = false;
    }

    /*public IEnumerator ScarfOut()
    {
        playerMove.animator.SetBool("IsScarfing", true);

        float startTime = Time.time;
        float EndTime = startTime + 0.4f;

        targetPos = new Vector3(xPos, startPos.y, 0);

        while(Time.time < EndTime)
        {
            float timeProg = (Time.time - startTime) / 0.4f;
            scarf.transform.localScale = Vector3.Lerp(baseScale, targetScale, timeProg * 8);
            scarf.transform.localPosition = Vector3.Lerp(startPos, targetPos, timeProg * 8);

            yield return new WaitForSeconds(0);
        }
        
        StartCoroutine(ScarfIn());
    }*/

    /*public IEnumerator ScarfIn()
    {        
        float startTime = Time.time;
        float EndTime = startTime + LerpTime;

        playerMove.animator.SetBool("IsScarfing", false);

        while(Time.time < EndTime)
        {
            float timeProg = (Time.time - startTime) / LerpTime;
            scarf.transform.localScale = Vector3.Lerp(targetScale, baseScale, timeProg * 10);
            scarf.transform.localPosition = Vector3.Lerp(targetPos, startPos, timeProg * 10);

            yield return new WaitForEndOfFrame();
            scarfOut = false;
        }
    }*/

    public IEnumerator Parry()
    {
        playerMove.animator.Play("Player_FullParry");
        CancelAttacks();
        parrying = true;
        GetComponent<Player_Movement>().animator.SetBool("IsRolling", false);
        GetComponent<Player_Movement>().rolling = 0;
        GetComponent<Player_Movement>().sfxSource.PlayOneShot(parrySound);
        canParry = false;
        GetComponent<Player_Movement>().gravityScale = 0f;
        GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x,0f,0f);
        GetComponent<Player_Movement>().canMove = false;
        yield return new WaitForSeconds(0.2f);
        parrying = false;
        yield return new WaitForSeconds(0.15f);
        if (!hookScarf.GetComponent<Hook_Behaviour>().hooked)
        {
            GetComponent<Player_Movement>().gravityScale = 1.7f;
        }
        GetComponent<Player_Movement>().canMove = true;
        yield return new WaitForSeconds(0.15f);
        canParry = true;
    }

    public IEnumerator ReturnAllThings()
    {
        yield return new WaitForSeconds(0.2f);
        canScarf = true;
        gunShot = false;
        canAttack = true;
        coReturn = null;
    }

    public void CancelAttacks()
    {
        if (coSword != null)
        {
            StopCoroutine(coSword);
            coSword = null;
        }
        if (coGun != null)
        {
            StopCoroutine(coGun);
            coGun = null;
        }
        if (coScarf != null)
        {
            StopCoroutine(coScarf);
            coScarf = null;
        }
        if (coReturn == null)
        {
            coReturn = StartCoroutine(ReturnAllThings());
        }
        playerMove.animator.SetBool("IsMeleeing", false);
        playerMove.animator.SetBool("IsShooting", false);
        playerMove.animator.SetBool("IsScarfing", false);
    }

    public void GetBullet()
    {
        if (!hasBullet)
        {
            Instantiate(bulletIndicator, transform);
        }
        hasBullet = true;
    }
    

    #endregion
}
