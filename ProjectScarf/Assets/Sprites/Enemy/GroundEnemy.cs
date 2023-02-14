using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : EnemyParents
{
    public float walkSpeed;
    public float WaitTime;
    public Transform[] posWalk;
    public float PlayerFindRange;
    private Transform playerTransform;

    private int i = 0;
    private bool movingTowardsLeft = true;
    private float waitingTIme;
    public float FollowSpeed;

    private Vector3 target;
    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();
        waitingTIme = WaitTime;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();
        FollowPlayer();
        if (playerTransform.position.x > transform.position.x)
        {
            target = playerTransform.position - new Vector3(1.0f, -1.0f, 0.0f);
        }
        else if (playerTransform.position.x < transform.position.x)
        {
            target = playerTransform.position + new Vector3(1.0f, 1.0f, 0.0f);
        }
        transform.position = Vector2.MoveTowards(transform.position, posWalk[i].position, walkSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, posWalk[i].position) < 0.1f)
        {
            if (WaitTime > 0)
            {
                WaitTime -= Time.deltaTime;
            }
            else
            {
                if (movingTowardsLeft)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingTowardsLeft = true;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingTowardsLeft = false;
                }
                if (i == 0)
                {
                    i = 1;
                }
                else
                {
                    i = 0;
                }
                WaitTime = waitingTIme;
            }
        }
    }

    void FollowPlayer()
    {
        if (playerTransform != null)
        {
            float distance = transform.position.x - playerTransform.position.x;
            if (Mathf.Abs(distance) < PlayerFindRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, FollowSpeed * Time.deltaTime);

            }
        }
    }
}
