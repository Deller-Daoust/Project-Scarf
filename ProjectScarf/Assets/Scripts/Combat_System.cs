using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_System : MonoBehaviour
{

    private Player_Movement playerMove;

    public GameObject scarf;
    
    private Vector3 targetScale;
    private Vector3 baseScale;

    private Vector3 startPos;
    private Vector3 targetPos;

    private Vector3 mousePos;
    private Vector3 mouseRot;
    //private float xMin = -1f, xMax = 1f;
    //private float yMin = -1f, yMax = 1f;
    private float xPos, yPos;

    public bool scarfOut = false;
    public bool gunShot = false;

    private float LerpTime = 1f;

    public Coroutine _scarfOut;

    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private Transform gun;
    private float gunRange = 0.5f;

    [SerializeField] private Transform sword;
    private float swordRange = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        targetScale = new Vector3(5, 0.5f, 0);

        baseScale = scarf.transform.localScale;

        startPos = scarf.transform.localPosition;

        playerMove = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && gunShot == false)
        {
            if(scarf.activeSelf == false)
            {
                scarf.SetActive(true);
            }

            scarfOut = true;
            _scarfOut = StartCoroutine(ScarfOut());
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
    public IEnumerator ScarfOut()
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
    }

    public IEnumerator ScarfIn()
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
    }
    

    #endregion
}
