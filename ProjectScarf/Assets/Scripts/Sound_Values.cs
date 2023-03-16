using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound_Values : MonoBehaviour
{
    [SerializeField] private AudioMixer masterVol;

    public float defaultVolume = 100f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Volume(defaultVolume);
    }

    public void Volume(float volumeLevel)
    {
        if(volumeLevel < 1)
        {
            volumeLevel = 0.001f;
        }

        masterVol.SetFloat("MasterVolume", Mathf.Log10(volumeLevel / 100) * 20f);
    }
}
