using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    bool canTrigger = true;
    float randomThing;
    // Start is called before the first frame update
    void Start()
    { 
        randomThing = ((float)Random.Range(0, 2) - 0.5f) * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().gravityScale != 0f)
        {
            transform.Rotate(0f, 0f, 30f * Time.deltaTime * -randomThing);
        }
    }

    void OnTriggerEnter2D()
    {
        if (canTrigger)
        {
            canTrigger = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(randomThing * 6f, 12f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(randomThing * 6f, 12f);
            GetComponent<Rigidbody2D>().gravityScale = 2f;
            GetComponent<AudioSource>().Play();
        }
    }
}
