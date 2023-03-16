using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Slider volSlider;

    private Sound_Values volController;

    // Start is called before the first frame update
    void Start()
    {
        volController = GameObject.Find("VolumeController").GetComponent<Sound_Values>();
        volSlider.value = volController.defaultVolume;
    }

    public void SliderVolume()
    {
        volController.Volume(volSlider.value);
    }
}
