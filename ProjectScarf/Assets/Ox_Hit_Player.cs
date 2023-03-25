using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ox_Hit_Player : MonoBehaviour
{
    public void HitPlayer(int _dmg = 2)
    {
        GameObject _player = GameObject.Find("Player");
        float tempDir=0f;
        
        _player.GetComponent<Player_Movement>().StartCoroutine(_player.GetComponent<Player_Movement>().GetHit(tempDir, _dmg));
    }
}
