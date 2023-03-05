using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Chomp : MonoBehaviour
{
    public ParticleSystem chompPS;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die",1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
