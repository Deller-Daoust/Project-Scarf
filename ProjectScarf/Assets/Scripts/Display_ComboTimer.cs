using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display_ComboTimer : MonoBehaviour
{
    GameObject player;
    public Image img;
    public Image img2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        img.fillAmount = Mathf.Lerp(img.fillAmount, player.GetComponent<Player_Movement>().comboTimer / 10f, 10f * Time.deltaTime);
        if (player.GetComponent<Player_Movement>().comboTimer == 0f)
        {
            img.enabled = false;
            img2.enabled = false;
        }
        else
        {
            img.enabled = true;
            img2.enabled = true;
        }
    }
}
