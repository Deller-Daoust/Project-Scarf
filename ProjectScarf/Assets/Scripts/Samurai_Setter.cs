using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Setter : MonoBehaviour
{
    private GameObject player;
    public GameObject textThing, ui, music;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnEnable()
    {
        GetComponent<Animator>().Play("draw");
        Invoke("EnableThing", 1.4f);
    }

    void EnableThing()
    {
        GetComponent<Samurai_Behaviour>().enabled = true;
        player.GetComponent<Player_Movement>().musicSource.enabled = true;
        player.GetComponent<Player_Movement>().canInput = true;
        textThing.SetActive(true);
        ui.SetActive(true);
        music.SetActive(true);
    }
}
