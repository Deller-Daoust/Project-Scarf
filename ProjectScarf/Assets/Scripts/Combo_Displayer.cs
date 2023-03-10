using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo_Displayer : MonoBehaviour
{
    public TextMeshProUGUI tex;
    private float playerCombo;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerCombo = player.GetComponent<Player_Movement>().combo;
        if (playerCombo > 0f)
        {
            tex.text = "COMBO: " + playerCombo.ToString();
        }
        else
        {
            tex.text = "";
        }
    }
}
