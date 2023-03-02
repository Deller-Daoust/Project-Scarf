using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty_Behaviour : MonoBehaviour
{

    [SerializeField] private GameObject pistolReticle;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("MakeReticle", 1f, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeReticle()
    {
        Instantiate(pistolReticle, new Vector2(0f, 0f), Quaternion.identity);
    }
}
