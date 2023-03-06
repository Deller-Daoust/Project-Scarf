using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dont_Destroy : MonoBehaviour
{
    private GameObject[] things;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        things = GameObject.FindGameObjectsWithTag("UI Thing");
        if (things.Length > 1)
        {
            Destroy(things[1]);
        }
    }
}
