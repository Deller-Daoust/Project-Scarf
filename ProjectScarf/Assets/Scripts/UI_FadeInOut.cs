using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_FadeInOut : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float speed = 0.02f;
    public float alpha = 0f;

    void Start()
    {
        text.color = new Color(1, 1, 1, 0);
        Invoke("SwitchSpeed",3f);
    }

    void FixedUpdate()
    {
        alpha += speed;
        if (alpha > 1f)
        {
            alpha = 1f;
        }
        text.color = new Color(1, 1, 1, alpha);
    }

    void SwitchSpeed()
    {
        speed = -speed;
    }
}
