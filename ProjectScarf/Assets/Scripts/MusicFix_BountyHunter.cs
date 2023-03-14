using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFix_BountyHunter : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip intro;
    // Start is called before the first frame update
    void OnEnable()
    {
        musicSource.PlayOneShot(intro);
        Invoke("PlayMusic",2.66f);
    }

    void PlayMusic()
    {
        musicSource.Play();
    }
}
