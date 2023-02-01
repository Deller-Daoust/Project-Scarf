using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHealthandDamage : EnemyParents
{
    public float walkSpeed;
    public float WaitTime;
    public Transform[] posWalk;


    private int i = 0;
    private bool movingTowardsLeft = true;
    private float waitingTIme;
    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();
        waitingTIme = WaitTime;
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();
        transform.position = Vector2.MoveTowards(transform.position, posWalk[i].position, walkSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position,posWalk[i].position )<0.1f)
        {
            if (WaitTime >0)
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
                if (i==0)
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
}
