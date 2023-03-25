using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth_Setting : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Renderer>().material.renderQueue = 3001;
    }
}
