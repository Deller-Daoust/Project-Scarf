using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score_Saver : MonoBehaviour
{
    private GameObject player;
    private Player_Movement playerContr;
    public int[] scores;
    public string[] ranks;
    private bool canFindPlayer;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        scores = new int[6];
        ranks = new string[6];
    }

    void Start()
    {
        //canFindPlayer = true;
    }

    void Update()
    {

      

        if (GameObject.FindWithTag("Player")!=null)
        {
            canFindPlayer = true;
        }
        if (GameObject.FindWithTag("Player")==null)
        {
            canFindPlayer = false;
        }

        if (canFindPlayer)
        {
            player = GameObject.FindWithTag("Player");
            playerContr = player.GetComponent<Player_Movement>();
        }
        Scene currentScene = SceneManager.GetActiveScene();

        if(player)
        {
            if (currentScene.name.Equals("TutorialLevel"))
            {
                scores[0] = playerContr.tempScore;
                ranks[0] = playerContr.rank;
            }
            if (currentScene.name.Equals("Level2"))
            {
                scores[1] = playerContr.tempScore;
                ranks[1] = playerContr.rank;
            }
            if (currentScene.name.Equals("Samurai"))
            {
                scores[2] = playerContr.tempScore;
                ranks[2] = playerContr.rank;
            }
            if (currentScene.name.Equals("Level3"))
            {
                scores[3] = playerContr.tempScore;
                ranks[3] = playerContr.rank;
            }
            if (currentScene.name.Equals("BountyHunter"))
            {
                scores[4] = playerContr.tempScore;
                ranks[4] = playerContr.rank;
            }
            
            if (currentScene.name.Equals("Level4"))
            {
                scores[5] = playerContr.tempScore;
                ranks[5] = playerContr.rank;
            }
            /*switch(currentScene.name)
            {
                case "TutorialLevel":
                    scores[0] = playerContr.tempScore;
                    ranks[0] = playerContr.rank;
                    break;
                case "Level2":
                    scores[1] = playerContr.tempScore;
                    ranks[1] = playerContr.rank;
                    break;
                case "Samurai":
                    scores[2] = playerContr.tempScore;
                    ranks[2] = playerContr.rank;
                    break;
                case "Level3":
                    scores[3] = playerContr.tempScore;
                    ranks[3] = playerContr.rank;
                    break;
                case "BountyHunter":
                    scores[4] = playerContr.tempScore;
                    ranks[4] = playerContr.rank;
                    break;
            }*/
        }
    }
}
