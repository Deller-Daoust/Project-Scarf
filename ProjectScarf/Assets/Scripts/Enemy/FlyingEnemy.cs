using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyParents
{
    public float speed;
    public float startTime;
    public float Times;

    public Transform NextPosition;

    public Transform RangeMove1;
    public Transform RangeMove2;

    public float Followspeed;
    public float radCheck;

    private Transform playertransform;

    private Vector2 target;
    // Start is called before the first frame update
    new public void Start()
    {

        base.Start();
        Times = startTime;
        NextPosition.position = GetRandomPos();
        playertransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    new public void Update()
    {
        if (playertransform.position.x > transform.position.x)
        {
            target = playertransform.position - new Vector3(1.0f, -1.0f, 0.0f);
        }
        else if (playertransform.position.x < transform.position.x)
        {
            target = playertransform.position + new Vector3(1.0f, 1.0f, 0.0f);
        }

        if (Times < -1)
        {
            Times = startTime;
        }
        base.Update();
        transform.position = Vector2.MoveTowards(transform.position, NextPosition.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, NextPosition.position) < 0.1f)
        {
            if (Times <= 0)
            {
                NextPosition.position = GetRandomPos();
                Times = startTime;
            }
            else
            {
                Times -= Time.deltaTime;
            }
        }

        if (playertransform != null)
        {
            float distance = (transform.position - playertransform.position).sqrMagnitude;

            if (distance < radCheck)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, Followspeed * Time.deltaTime);
            }
        }
    }

    Vector2 GetRandomPos()
    {
        Vector2 randomPos = new Vector2(Random.Range(RangeMove1.position.x, RangeMove2.position.x), Random.Range(RangeMove1.position.y, RangeMove2.position.y));
        return randomPos;

    }
}
