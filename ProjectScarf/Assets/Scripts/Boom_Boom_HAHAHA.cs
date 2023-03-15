using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Boom_HAHAHA : MonoBehaviour
{
    public GameObject explosion;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnExplosion",0f,0.04f);
        InvokeRepeating("PlaySound",0f,0.12f);
        source = GetComponent<AudioSource>();
    }

    void SpawnExplosion()
    {
        Instantiate(explosion,new Vector2(Random.Range(-9f,9f),Random.Range(5f,-5f)),Quaternion.identity);
    }

    void PlaySound()
    {
        source.Play();
    }
}
