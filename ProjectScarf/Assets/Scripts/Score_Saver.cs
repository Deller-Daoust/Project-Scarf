using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score_Saver : MonoBehaviour
{
    private GameObject player;
    private Player_Movement playerContr;
    public int[] scores;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        scores = new int[5];
    }

    void Start()
    {
        player = GameObject.Find("Player");
        playerContr = player.GetComponent<Player_Movement>();
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if(player)
        {
            switch(currentScene.name)
            {
                case "TutorialLevel":
                    scores[0] = playerContr.tempScore;
                    break;
                case "Level2":
                    scores[1] = playerContr.tempScore;
                    break;
                case "Samurai":
                    scores[2] = playerContr.tempScore;
                    break;
                case "Level3":
                    scores[3] = playerContr.tempScore;
                    break;
                case "BountyHunter":
                    scores[4] = playerContr.tempScore;
                    break;
            }
        }
    }
}
