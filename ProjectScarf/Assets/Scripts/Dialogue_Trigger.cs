using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    public GameObject dialoguebox;
    public SpriteRenderer spriteRenderer;
    private GameObject player;
    private bool triggered;
    public Sprite finishedSprite;
    public GameObject light;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.enabled = false;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (triggered)
        {
            GetComponent<Animator>().enabled = false;
            spriteRenderer.sprite = finishedSprite;
            light.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            triggered = true;
            dialoguebox.SetActive(true);
        }
    }
}
