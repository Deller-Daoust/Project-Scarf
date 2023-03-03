using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Sprite[] healthSprites;

    private GameObject player;
    private Combat_System combat;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        combat = player.GetComponent<Combat_System>();
    }

    // Update is called once per frame
    void Update()
    {
        int hp = combat.hp;

        switch(hp)
        {
            case 6:
                healthBar.GetComponent<Image>().sprite = healthSprites[6];
                break;
            case 5:
                healthBar.GetComponent<Image>().sprite = healthSprites[5];
                break;
            case 4:
                healthBar.GetComponent<Image>().sprite = healthSprites[4];
                break;
            case 3:
                healthBar.GetComponent<Image>().sprite = healthSprites[3];
                break;
            case 2:
                healthBar.GetComponent<Image>().sprite = healthSprites[2];
                break;
            case 1:
                healthBar.GetComponent<Image>().sprite = healthSprites[1];
                break;
            case 0:
                healthBar.GetComponent<Image>().sprite = healthSprites[0];
                break;
        }
    }
}
