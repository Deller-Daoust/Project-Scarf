using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{

    public GameObject canvy;
    public AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateBluescreen",3f);
    }

    void ActivateBluescreen()
    {
        canvy.SetActive(true);
        musicSource.Play();
    }
}
