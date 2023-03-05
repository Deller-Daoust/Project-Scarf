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

    private Vector3 mousePos;
    private Vector3 mouseRot;
    //private float xMin = -1f, xMax = 1f;
    //private float yMin = -1f, yMax = 1f;
    private float xPos, yPos;

    public bool scarfOut = false;
    public bool gunShot = false;
    public bool parrying = false;
    public bool canParry = true;
    private bool canAttack = true;

    public GameObject hitbox;

    //private float LerpTime = 1f;

    //public Coroutine _scarfOut;

    [SerializeField] private LayerMask enemyLayers;

    [Header("Sounds")]
    [SerializeField] private AudioClip parrySound, chompSound;

    [Header("Snake chomp")]
    public GameObject chompDude;
    private GameObject activeDude;

    [Header("Gun")]
    [SerializeField] private Transform gun;
    private float gunRange = 0.5f;
    public bool hasBullet = false;
    public GameObject bulletIndicator;

    [SerializeField] private Transform sword;
    private float swordRange = 0.85f;

    [SerializeField] private Transform scarf;
    private Vector2 scarfRange = new Vector2(8f, 3f);

    private bool canScarf = true;

    // Start is called before the first frame update
    void Start()
    {
        targetScale = new Vector3(5, 0.5f, 0);

        /*baseScale = scarf.transform.localScale;

        startPos = scarf.transform.localPosition;*/

        playerMove = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hp);
        if(Input.GetKeyDown(KeyCode.F) && gunShot == false)
        {
            if(canScarf)
            {
                StartCoroutine(Scarf());
                StartCoroutine(ScarfOut());
            }
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            if(canParry)
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

        if(Input.GetMouseButtonDown(0) && scarfOut == false && gunShot == false && canAttack)
        {
            StartCoroutine(SwordAttack());
        }

        if(Input.GetMouseButtonDown(1) && scarfOut == false && hasBullet)
        {
            gunShot = true;
            hasBullet = false;
            StartCoroutine(GunBlast());
        }
    }

    #region Attacks

    IEnumerator Scarf()
    {
        canScarf = false;

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
                activeDude = Instantiate(chompDude, closestEnemy.transform.position, Quaternion.identity);
                activeDude.GetComponent<Snake_Chomp>().target = closestEnemy;
                yield return new WaitForSeconds(0.46f/1.15f);
                activeDude.GetComponent<SpriteRenderer>().sprite = activeDude.GetComponent<Snake_Chomp>().chompedSprite;
                GetComponent<Player_Movement>().sfxSource.PlayOneShot(chompSound);
                Invoker.InvokeDelayed(ResumeTime,0.15f);
                Time.timeScale = 0f;
                closestEnemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                activeDude.GetComponent<Snake_Chomp>().chompPS.Play();
                yield return new WaitForSeconds(0.01f);
                playerMove.transform.position = new Vector2 (closestEnemy.transform.position.x - (playerMove.playerDir * 1.2f), closestEnemy.transform.position.y);
                closestEnemy = null;
                yield return new WaitForSeconds(0.15f);
                activeDude.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(1f);
                Destroy(activeDude);
            }
        }

        yield return new WaitForSeconds(0.8f); // Cooldown

        canScarf = true;
    }

    void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    IEnumerator ScarfOut()
    {
        playerMove.canMove = false;

        playerMove.animator.Play("Player_Scarf");

        yield return new WaitForSeconds(0.8f);

        playerMove.canMove = true;
    }

    IEnumerator SwordAttack()
    {
        GetComponent<Player_Movement>().canMove = false;
        GetComponent<Player_Movement>().rolling = 0;
        playerMove.animator.SetBool("IsMeleeing", true);
        canAttack = false;
        
        yield return new WaitForSeconds(0.1f);
        HitEnemies(2);
        yield return new WaitForSeconds(0.1f);
        HitEnemies(2, 10f, 0.08f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<Player_Movement>().canMove = true;
        canAttack = true;
    }

    void HitEnemies(int _damage, float _force = 3f, float _stun = 0.05f)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(sword.position, swordRange, enemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            Debug.Log("Melee Hit: " + enemy.name);
            if (enemy.GetComponent<Flash_Functions>() != null)
            {
                enemy.GetComponent<Flash_Functions>().flashSprite(Color.red);
            }
            if (enemies[0] != null)
            {
                if (_stun == 0.08f)
                {
                    GetBullet();
                }
            }
            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(_force * GetComponent<Player_Movement>().playerDir, 0f);
            Time.timeScale = 0f;
            Invoker.InvokeDelayed(ResumeTime, _stun);
        }
    }

    IEnumerator GunBlast()
    {
        GetComponent<Player_Movement>().canMove = false;
        GetComponent<Player_Movement>().rolling = 0;
        playerMove.animator.SetBool("IsShooting", true);
        yield return new WaitForSeconds(0.35f);
        HitEnemies(4, 10f, 0.12f);
        yield return new WaitForSeconds(0.3f);
        GetComponent<Player_Movement>().canMove = true;
        gunShot = false;
    } 

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(sword.position, swordRange);
        Gizmos.DrawWireSphere(gun.position, gunRange);
        Gizmos.DrawWireCube(scarf.position, scarfRange);
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

    private IEnumerator Parry()
    {
        parrying = true;
        GetComponent<Player_Movement>().sfxSource.PlayOneShot(parrySound);
        canParry = false;
        GetComponent<Player_Movement>().gravityScale = 0f;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);
        GetComponent<Player_Movement>().canMove = false;
        yield return new WaitForSeconds(0.2f);
        parrying = false;
        yield return new WaitForSeconds(0.25f);
        GetComponent<Player_Movement>().gravityScale = 1.7f;
        GetComponent<Player_Movement>().canMove = true;
        yield return new WaitForSeconds(0.1f);
        canParry = true;
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
