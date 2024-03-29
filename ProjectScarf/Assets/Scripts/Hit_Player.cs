using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Player : MonoBehaviour
{
    public void HitPlayer(int _dmg = 1)
    {
        GameObject _player = GameObject.Find("Player");
        float tempDir;
        if (transform.position.x > _player.transform.position.x)
        {
            tempDir = -1f;
        }
        else
        {
            tempDir = 1f;
        }
        _player.GetComponent<Player_Movement>().StartCoroutine(_player.GetComponent<Player_Movement>().GetHit(tempDir, _dmg));
    }
}
