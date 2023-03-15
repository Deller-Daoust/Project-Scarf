using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScream : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] screams;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = screams[Random.Range(0,screams.Length)];
    }

    public void Scream()
    {
        source.Play();
    }
}
