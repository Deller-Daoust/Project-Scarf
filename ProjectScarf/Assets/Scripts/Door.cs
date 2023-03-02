using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string scene;

    void OnTriggerEnter2D()
    {
        gotoScene();
    }

    void gotoScene()
    {
        SceneManager.LoadScene(scene);
    }
}
