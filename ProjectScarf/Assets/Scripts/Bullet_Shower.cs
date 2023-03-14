using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_Shower : MonoBehaviour
{
    private GameObject player;
    public RawImage img;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Combat_System>().hasBullet)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }
    }
}
