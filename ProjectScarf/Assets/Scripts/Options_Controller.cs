using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options_Controller : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    public void Open()
    {
        if(optionsMenu.activeSelf == false)
        {
            optionsMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        if(optionsMenu.activeSelf == true)
        {
            optionsMenu.SetActive(false);
        }
    }
}
