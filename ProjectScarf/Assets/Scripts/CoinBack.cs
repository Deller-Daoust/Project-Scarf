using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinBack : MonoBehaviour
{
    public Sprite[] sprites;
    private Image image;
    public int cReq, bReq, aReq, sReq;
    private AudioSource source;
    public Player_Movement pm;
    public Sprite oldSprite;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.FindWithTag("Player").GetComponent<Player_Movement>();
        cReq = pm.cReq;
        bReq = pm.bReq;
        aReq = pm.aReq;
        sReq = pm.sReq;
        image = GetComponent<Image>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        oldSprite = image.sprite;
        if (pm.tempScore < cReq)
        {
            image.sprite = sprites[0];
        }
        else if (pm.tempScore < bReq)
        {
            image.sprite = sprites[1];
        }
        else if (pm.tempScore < aReq)
        {
            image.sprite = sprites[2];
        }
        else if (pm.tempScore < sReq)
        {
            image.sprite = sprites[3];
        }
        else
        {
            image.sprite = sprites[4];
        }
        if (GetComponent<CoinFront>() != null && oldSprite != image.sprite)
        {
            anim.Play("Combo_Spin");
            source.Play();
            GetComponent<CoinFront>().image.fillAmount = 0f;
        }
    }
}
