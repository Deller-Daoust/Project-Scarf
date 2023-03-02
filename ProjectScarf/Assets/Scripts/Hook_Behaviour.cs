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

    private float angleRad;
    private float angleDeg;

    public float distance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            hookCollider = Physics2D.OverlapBox(transform.position, hookCheck, 0, hookLayer);

            if(hookCollider)
            {
                angleRad = Mathf.Atan2(spawnPoint.transform.position.y - hookCollider.transform.position.y, spawnPoint.transform.position.x - hookCollider.transform.position.x);
                angleDeg = (180 / Mathf.PI) * angleRad;

                distance = Vector3.Distance(spawnPoint.transform.position, hookCollider.transform.position);

                GameObject hookScarf = Instantiate(sprite, spawnPoint.transform.position, Quaternion.Euler(0, 0, angleDeg));
                hookScarf.transform.parent = gameObject.transform;
                hookScarf.GetComponent<SpriteRenderer>().flipX = true;

                if(angleDeg < -90)
                {
                    hookScarf.GetComponent<SpriteRenderer>().flipY = true;
                }
                else if(angleDeg > -90)
                {
                    hookScarf.GetComponent<SpriteRenderer>().flipY = false;
                }
                

                //GameObject scarfSprite = Instantiate(sprite, transform.position, Quaternion.LookRotation(player.transform.position - hookCollider.transform.position));
                //scarfSprite.transform.Rotate(0f, 0f, angleDeg);

                //Debug.Log(angleDeg);
            }

            Debug.Log(hookCollider);
        }   
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, hookCheck);
    }
}
