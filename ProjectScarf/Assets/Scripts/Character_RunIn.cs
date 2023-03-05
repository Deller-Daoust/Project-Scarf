using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_RunIn : MonoBehaviour
{
    private Player_Movement move;
    public float timeToMove, dir;

    void Start()
    {
        move = GetComponent<Player_Movement>();
        move.canInput = false;
        move.moveInput = new Vector2(dir, 0f);
        Invoke("StopRunning",timeToMove);
    }
    
    
    void StopRunning()
    {
        move.moveInput = Vector2.zero;
    }
}
