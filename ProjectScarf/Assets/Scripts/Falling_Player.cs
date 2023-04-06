using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Player : MonoBehaviour
{
    public AudioSource source;
    public Sprite newSprite;
    public GameObject music, ui;
    public bool canLand;
    public GameObject sprite;

    void Start()
    {
        Invoke("CanLand", 1f);
    }

    void CanLand()
    {
        canLand = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<Rigidbody2D>().velocity.y < 0f)
        {
            transform.eulerAngles += new Vector3(0, 0, -200f) * Time.deltaTime;
        }
        else
        {
            if (canLand)
            {
                canLand = false;
                source.Play();
                transform.eulerAngles = Vector3.zero;
                sprite.GetComponent<SpriteRenderer>().sprite = newSprite;
                sprite.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }

        if (transform.parent.GetComponent<Player_Movement>().canInput)
        {
            transform.parent.GetComponent<SpriteRenderer>().enabled = true;
            transform.parent.GetComponent<Player_Movement>().musicSource.Play();
            music.SetActive(true);
            ui.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
