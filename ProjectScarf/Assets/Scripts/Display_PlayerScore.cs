using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Display_PlayerScore : MonoBehaviour
{
    GameObject player;
    public TextMeshProUGUI tex;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        tex.text = player.GetComponent<Player_Movement>().tempScore.ToString();
    }

}
