using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crit_Dummy : MonoBehaviour
{
    public GameObject door;
    
    void OnDestroy()
    {
        Destroy(door);
    }
}
