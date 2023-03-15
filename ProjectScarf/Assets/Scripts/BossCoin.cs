using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCoin : MonoBehaviour
{
    public Sprite[] sprites;
    private Image image;
    public int cReq, bReq, aReq, sReq;
    private AudioSource source;
    public Combat_System cs;
    public Sprite oldSprite;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        cs = GameObject.FindWithTag("Player").GetComponent<Combat_System>();
        image = GetComponent<Image>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        oldSprite = image.sprite;
        if (cs.totalHealthLost > cReq)
        {
            image.sprite = sprites[0];
        }
        else if (cs.totalHealthLost > bReq)
        {
            image.sprite = sprites[1];
        }
        else if (cs.totalHealthLost > aReq)
        {
            image.sprite = sprites[2];
        }
        else if (cs.totalHealthLost > sReq)
        {
            image.sprite = sprites[3];
        }
        else
        {
            image.sprite = sprites[4];
        }
        if (oldSprite != image.sprite)
        {
            anim.Play("Combo_Spin");
            source.Play();
        }
    }
}
