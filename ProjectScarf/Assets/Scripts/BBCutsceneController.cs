using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BBCutsceneController : MonoBehaviour
{
    public GameObject bigBusiness, bigSprite;
    public SpriteRenderer player;
    public GameObject playerCorpse;


    public void DoThing()
    {
        StartCoroutine(BigWalk());
    }

    IEnumerator BigWalk()
    {
        yield return new WaitForSeconds(0.4f);
        bigBusiness.GetComponent<Animator>().Play("RunToPlayer");
        yield return new WaitForSeconds(1.05f);
        bigSprite.GetComponent<Animator>().Play("Big_Toss");
        yield return new WaitForSeconds(0.675f);
        player.enabled = false;
        yield return new WaitForSeconds(1.433f);
        playerCorpse.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
