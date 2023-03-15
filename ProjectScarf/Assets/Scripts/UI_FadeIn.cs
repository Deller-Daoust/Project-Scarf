using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_FadeIn : MonoBehaviour
{
    public Image img, img2;
    public float speed = 0.02f;
    public float alpha = 0f;

    void Start()
    {
        img.color = new Color(1, 1, 1, 0);
        img2.color = new Color(1, 1, 1, 0);
    }

    void FixedUpdate()
    {
        alpha += speed;
        if (alpha > 1f)
        {
            alpha = 1f;
        }
        img.color = new Color(1, 1, 1, alpha);
        img2.color = new Color(1, 1, 1, alpha);
    }
}
