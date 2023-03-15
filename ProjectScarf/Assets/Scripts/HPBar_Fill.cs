using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar_Fill : MonoBehaviour
{
    public Image bar;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = (float)target.GetComponent<HP_Handler>().health / (float)target.GetComponent<HP_Handler>().maxHealth;
    }
}
