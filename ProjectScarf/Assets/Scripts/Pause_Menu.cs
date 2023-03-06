using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public bool gameIsPaused = false;
    private GameObject player;
    private AudioSource[] allAudioSources;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu)
            {
                if(pauseMenu.activeSelf == true)
                {
                    Resume();
                }
                else
                {
                    StopAllAudio();
                    GetComponent<AudioSource>().Play();
                    player.GetComponent<Player_Movement>().musicSource.Pause();
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    gameIsPaused = true;
                }
            }
        }
    }

    public void Resume()
    {
        GetComponent<AudioSource>().Stop();
        foreach(AudioSource audioS in allAudioSources)
        {
            audioS.UnPause();
        }
        //player.GetComponent<Player_Movement>().musicSource.UnPause();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void StopAllAudio()
    {
    allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    foreach(AudioSource audioS in allAudioSources)
    {
        audioS.Pause();
    }
 }
}
