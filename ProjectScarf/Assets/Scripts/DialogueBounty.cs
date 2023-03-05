using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueBounty : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public AudioSource source;
    public AudioClip textSound;

    private int index;

    public GameObject stupidThing, otherStupidThing, otherStupidThing2, otherStupidThing3;
    private GameObject player, bountyHunter;
    private AudioSource playerMusic;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        bountyHunter = GameObject.Find("Bounty Hunter");
        otherStupidThing.SetActive(false);
        otherStupidThing2.SetActive(false);
        otherStupidThing3.SetActive(false);
        playerMusic = player.GetComponent<Player_Movement>().musicSource;
    }

    void StartDisc()
    {
        stupidThing.SetActive(true);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject[] toDisable = GameObject.FindGameObjectsWithTag("DialogueBox");
        for (int i = 0; i < toDisable.Length; i++)
        {
            if (toDisable[i] != gameObject)
            {
                toDisable[i].SetActive(false);
            }
        }
        textComponent.text = string.Empty;
        startDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (textComponent.text == lines[index])
            {
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //try {
                //playerMusic.Play();
            //} 
            //catch (Exception E)
            //{

            //}
            //bountyHunter.GetComponent<Bounty_Behaviour>().enabled = true;
            //Invoke("StartDisc", 2.66f);
            //otherStupidThing.SetActive(true);
            //otherStupidThing2.SetActive(true);
            //otherStupidThing3.SetActive(true);
            //gameObject.SetActive(false);
        }
    }

    void startDialogue()
    {
        index = 0;
        StartCoroutine(typeLine());
    }

    IEnumerator typeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            if (c != ' ')
            {
                source.PlayOneShot(textSound);
            }
            textComponent.text += c;
            if (c == '.' || c == '!' || c == '?')
            {
                yield return new WaitForSeconds(textSpeed * 5);
            }
            else if (c == ',')
            {
                yield return new WaitForSeconds(textSpeed * 3);
            }
            else
            {
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }

    void nextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(typeLine());
        }
        else
        {
            player.GetComponent<Player_Movement>().musicSource.Play();
            bountyHunter.GetComponent<Bounty_Behaviour>().enabled = true;
            //Invoke("StartDisc", 2.66f);
            otherStupidThing.SetActive(true);
            otherStupidThing2.SetActive(true);
            otherStupidThing3.SetActive(true);
            //gameObject.SetActive(false);
        }
    }
}
