using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rating_Screen : MonoBehaviour
{
    public int score;
    public string rank;
    public GameObject scoreSaver, text;
    private Animator anim;
    public AudioSource source;
    public AudioClip dRank, cRank, bRank, aRank, sRank, drumroll;
    public int roomNumber;
    private bool canUpdate;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DoUpdateThing", 3f);
        scoreSaver = GameObject.FindWithTag("Score Saver");
        anim = GetComponent<Animator>();
    }

    void DoUpdateThing()
    {
        canUpdate = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (canUpdate)
        {
            canUpdate = false;
            score = scoreSaver.GetComponent<Score_Saver>().scores[roomNumber];
            rank = scoreSaver.GetComponent<Score_Saver>().ranks[roomNumber];
            text.SetActive(true);
            if (rank.Equals("D"))
            {
                anim.Play("D");
                source.clip = dRank;
                source.Play();
            }
            if (rank.Equals("C"))
            {
                anim.Play("C");
                source.clip = cRank;
                source.Play();
            }
            if (rank.Equals("B"))
            {
                anim.Play("B");
                source.clip = bRank;
                source.Play();
            }
            if (rank.Equals("A"))
            {
                anim.Play("A");
                source.clip = aRank;
                source.Play();
            }
            if (rank.Equals("S"))
            {
                anim.Play("S");
                source.clip = sRank;
                source.Play();
            }
        }
    }
}
