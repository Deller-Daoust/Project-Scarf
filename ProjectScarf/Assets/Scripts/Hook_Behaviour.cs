using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook_Behaviour : MonoBehaviour
{
    [SerializeField] private Vector2 hookCheck;

    [SerializeField] LayerMask hookLayer;

    private Collider2D hookCollider;

    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject player;
    private GameObject hookScarf;

    private float angleRad;
    private float angleDeg;

    private bool hooked;
    private bool hookSent;

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(hookSent == false)
        {
            if(Input.GetKeyDown(KeyCode.E) && hooked == false)
            {
                hookCollider = Physics2D.OverlapBox(transform.position, hookCheck, 0, hookLayer);

                if(hookCollider)
                {
                    hookSent = true;
                    angleRad = Mathf.Atan2(spawnPoint.transform.position.y - hookCollider.transform.position.y, spawnPoint.transform.position.x - hookCollider.transform.position.x);
                    angleDeg = (180 / Mathf.PI) * angleRad;

                    distance = Vector3.Distance(spawnPoint.transform.position, hookCollider.transform.position);

                    hookScarf = Instantiate(sprite, spawnPoint.transform.position, Quaternion.Euler(0, 0, angleDeg));
                    hookScarf.transform.parent = gameObject.transform;
                    hookScarf.GetComponent<SpriteRenderer>().flipX = true;
                    if (player.GetComponent<Player_Movement>().facingRight)
                    {
                        hookScarf.GetComponent<SpriteRenderer>().flipX = false;
                    }


                    if(angleDeg < -90)
                    {
                        hookScarf.GetComponent<SpriteRenderer>().flipY = true;
                    }
                    else if(angleDeg > -90)
                    {
                        hookScarf.GetComponent<SpriteRenderer>().flipY = false;
                    }

                    StartCoroutine(HookTele(hookCollider.transform.position));
                }
            }
        }

        if(player.GetComponent<Player_Movement>().gravityScale == 0f && hooked)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetKeyDown(KeyCode.Space))
            {
                player.GetComponent<Player_Movement>().gravityScale = 1.7f;
                player.GetComponent<Player_Movement>().Jump();
                hooked = false;
                hookSent = false;
            }
        }
        
    }

    IEnumerator HookTele(Vector3 hook)
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<Player_Movement>().canMove = false;
        yield return new WaitForSeconds(0.34f);
        Invoker.InvokeDelayed(ResumeTime,0.1f);
        Time.timeScale = 0f;
        yield return new WaitForSeconds(0.06f);
        hooked = true;
        player.GetComponent<Player_Movement>().gravityScale = 0f;
        player.transform.position = hook;
        Destroy(hookScarf);
        player.GetComponent<Player_Movement>().canMove = true;
    }

    void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, hookCheck);
    }
}
