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

        healthBar.GetComponent<Image>().sprite = healthSprites[hp];
    }
}
