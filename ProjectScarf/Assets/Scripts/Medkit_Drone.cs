using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit_Drone : MonoBehaviour
{
    public GameObject medkit;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DropKit", 1.25f);
        Invoke("Die", 5f);
    }

    void DropKit()
    {
        Instantiate(medkit, transform.position, Quaternion.identity);
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}
