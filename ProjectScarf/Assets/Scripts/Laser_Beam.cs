using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Beam : MonoBehaviour
{
    private GameObject player;
    public GameObject dialogue;

    void Start()
    {
        player = GameObject.Find("Player");
        //dialogue = GameObject.Find("DialogueBox - laserdeath");
    }

    void OnTriggerEnter2D()
    {
        //player.GetComponent<Combat_System>().hp = 0;
        dialogue.SetActive(true);
        player.transform.position = new Vector3(75.55193f,3.83539f,0f);
    }
}
