using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpeedReturner : MonoBehaviour
{
    public Player_Movement player;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CanInput", 5f);
    }

    // Update is called once per frame
    void CanInput()
    {
        player.canInput = true;
    }
}
