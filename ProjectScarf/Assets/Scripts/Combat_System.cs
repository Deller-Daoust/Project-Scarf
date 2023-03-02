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

    //private float LerpTime = 1f;

    //public Coroutine _scarfOut;

    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private Transform gun;
    private float gunRange = 0.5f;

    [SerializeField] private Transform sword;
    private float swordRange = 0.75f;

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

        if(Input.GetMouseButtonDown(0) && scarfOut == false && gunShot == false)
        {
            SwordAttack();
        }

        if(Input.GetMouseButtonDown(1) && scarfOut == false)
        {
            gunShot = true;
            GunBlast();
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
                yield return new WaitForSeconds(0.44f);
                Invoker.InvokeDelayed(ResumeTime,0.1f);
                Time.timeScale = 0f;
                yield return new WaitForSeconds(0.06f);

                playerMove.transform.position = closestEnemy.transform.position;

                closestEnemy = null;
            }
        }

        yield return new WaitForSeconds(1f); // Cooldown

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

        yield return new WaitForSeconds(1f);

        playerMove.canMove = true;
    }

    void SwordAttack()
    {
        playerMove.animator.SetBool("IsMeleeing", true);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(sword.position, swordRange, enemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            Debug.Log("Melee Hit: " + enemy.name);
        }
    }

    void GunBlast()
    {
        playerMove.animator.SetBool("IsShooting", true);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(gun.position, gunRange, enemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            Debug.Log("Gun Shot: " + enemy.name);
        }

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
    

    #endregion
}
