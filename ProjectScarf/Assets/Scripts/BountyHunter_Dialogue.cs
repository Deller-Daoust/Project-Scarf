using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunter_Dialogue : MonoBehaviour
{
    public GameObject stupidThing, otherStupidThing, otherStupidThing2, otherStupidThing3;
    private GameObject player, bountyHunter, uiThing;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        bountyHunter = GameObject.Find("Bounty Hunter");
        uiThing = GameObject.Find("UI");
        otherStupidThing.SetActive(false);
        otherStupidThing2.SetActive(false);
        otherStupidThing3.SetActive(false);
    }

    void OnDisable()
    {
        player.GetComponent<Player_Movement>().musicSource.Play();
        bountyHunter.GetComponent<Bounty_Behaviour>().enabled = true;
        Invoke("StartDisc", 2.66f);
        otherStupidThing.SetActive(true);
        otherStupidThing2.SetActive(true);
        otherStupidThing3.SetActive(true);
    }

    void StartDisc()
    {
        player.GetComponent<Player_Movement>().canInput = true;
        stupidThing.SetActive(true);
    }
}
