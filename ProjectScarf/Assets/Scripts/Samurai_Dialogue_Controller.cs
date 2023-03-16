using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Dialogue_Controller : MonoBehaviour
{
    GameObject samurai;
    // Start is called before the first frame update
    void Start()
    {
        samurai = GameObject.Find("Samurai");
    }

    void OnDisable()
    {
        samurai.GetComponent<Samurai_Setter>().enabled = true;
    }
}
