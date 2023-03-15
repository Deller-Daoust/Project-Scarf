using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingAnt : MonoBehaviour
{
    HP_Handler hp;
    public GameObject ant, soda;
    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<HP_Handler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp.health < 1000)
        {
            Instantiate(soda, transform.position, Quaternion.identity);
            Instantiate(ant, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
