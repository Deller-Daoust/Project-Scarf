using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueCutscene : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public Sprite[] images;
    public float textSpeed;
    public AudioSource source;
    public AudioClip textSound;
    public Image spr;
    public GameObject bigBusiness;
    public GameObject bigSprite;

    private int index;

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
        if (images.Length > 0)
        {
            spr.sprite = images[0];
        }
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
            gameObject.SetActive(false);
            bigSprite.GetComponent<Animator>().Play("Big_Walk");
            bigBusiness.GetComponent<Animator>().Play("RunToPlayer");
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
            if (images.Length > 0)
            {
                spr.sprite = images[index];
            }
            StartCoroutine(typeLine());
            if (index == 1)
            {
                bigBusiness.GetComponent<Animator>().Play("FlyInScene");
            }
        }
        else
        {
            gameObject.SetActive(false);
            bigSprite.GetComponent<Animator>().Play("Big_Walk");
            StartCoroutine(BigWalk());
        }
    }

    IEnumerator BigWalk()
    {
        yield return new WaitForSeconds(0.7f);
        bigBusiness.GetComponent<Animator>().Play("RunToPlayer");
        yield return new WaitForSeconds(1.5f);
        bigSprite.GetComponent<Animator>().Play("Big_Grab");
    }
}
