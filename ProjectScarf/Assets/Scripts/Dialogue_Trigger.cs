using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    public GameObject dialoguebox;
    public SpriteRenderer spriteRenderer;
    private GameObject player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            dialoguebox.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
