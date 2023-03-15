using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextVanish_HpAppear : MonoBehaviour
{
    private UI_FadeInOut otherScript;
    public GameObject hpBar;
    // Start is called before the first frame update
    void Start()
    {
        otherScript = GetComponent<UI_FadeInOut>();
    }

    // Update is called once per frame
    void Update()
    {
        if (otherScript.alpha < 0f && otherScript.speed < 0f)
        {
            Destroy(otherScript.gameObject);
            hpBar.SetActive(true);
        }
    }
}
