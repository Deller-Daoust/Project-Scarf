using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Real_Laser_Beam : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D()
    {
        player.GetComponent<Combat_System>().hp = 0;
    }
}
