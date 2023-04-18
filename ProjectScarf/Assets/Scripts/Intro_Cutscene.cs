using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro_Cutscene : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject text;

    void Start()
    {
        Invoke("EndScene", 35f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       
       if (video.frame >= 1100)
       {
           text.SetActive(true);
           Debug.Log("bruh");
       }
    }

    void EndScene()
    {
        text.SetActive(true);
    }
}
