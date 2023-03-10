using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{

    public GameObject canvy;
    public AudioSource musicSource;
    private AudioSource[] allAudioSources;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateBluescreen",2f);
    }

    void ActivateBluescreen()
    {
        canvy.SetActive(true);
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach(AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
        musicSource.Play();
    }
}
