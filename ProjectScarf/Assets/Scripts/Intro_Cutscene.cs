using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro_Cutscene : MonoBehaviour
{
    public VideoPlayer video;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void EndReached(VideoPlayer video)
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }
}
