using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_System : MonoBehaviour
{
    private Player_Movement playerMove;

    public GameObject scarf;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;
    private Vector3 targetScale;
    private Vector3 baseScale;

    private Vector3 startPos;
    private Vector3 targetPos;

    private Vector3 mousePos;
    private Vector3 mouseRot;
    //private float xMin = -1f, xMax = 1f;
    //private float yMin = -1f, yMax = 1f;
    private float xPos, yPos;

    private bool scarfOut = false;
    private bool swordStrike = false;
    private bool gunShot = false;

    private float LerpTime = 1f;

    public Coroutine _scarfOut;

    // Start is called before the first frame update
    void Start()
    {
        targetScale = new Vector3(2, 0.1f, 0);

        baseScale = scarf.transform.localScale;

        startPos = scarf.transform.localPosition;

        playerMove = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && swordStrike == false && gunShot == false)
        {
            if(scarf.activeSelf == false)
            {
                scarf.SetActive(true);
            }

            scarfOut = true;
            _scarfOut = StartCoroutine(ScarfOut());
        }

        /*mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseRot = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        xPos = Mathf.Clamp(mousePos.x, xMin, xMax);
        yPos = Mathf.Clamp(mousePos.y, yMin, yMax);*/

        /*if(playerMove.facingRight == true)
        {
            xPos *= -1;
        }*/

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

        

        if(Input.GetButtonDown("Fire1") && scarfOut == false && gunShot == false)
        {
            swordStrike = true;
            StartCoroutine(SwordAttack());
        }

        if(Input.GetButtonDown("Fire2") && scarfOut == false && swordStrike == false)
        {
            gunShot = true;
            StartCoroutine(GunBlast());
        }
    }

    #region Attacks
    public IEnumerator ScarfOut()
    {
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

        while(Time.time < EndTime)
        {
            float timeProg = (Time.time - startTime) / LerpTime;
            scarf.transform.localScale = Vector3.Lerp(targetScale, baseScale, timeProg * 10);
            scarf.transform.localPosition = Vector3.Lerp(targetPos, startPos, timeProg * 10);

            yield return new WaitForEndOfFrame();
            scarfOut = false;
        }
    }

    IEnumerator SwordAttack()
    {
        sword.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        sword.SetActive(false);
        swordStrike = false;
    }

    IEnumerator GunBlast()
    {
        gun.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        gun.SetActive(false);
        gunShot = false;
    } 
    
    #endregion
}
