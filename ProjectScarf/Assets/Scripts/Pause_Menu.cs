using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public bool gameIsPaused = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu)
            {
                if(pauseMenu.activeSelf == true)
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                    gameIsPaused = false;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    gameIsPaused = true;
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
}
