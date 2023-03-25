using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinFront : MonoBehaviour
{
    public Image image;
    CoinBack cb;
    // Start is called before the first frame update
    void Start()
    {
        cb = GetComponent<CoinBack>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cb.pm.tempScore < cb.cReq)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, (float)cb.pm.tempScore / (float)cb.cReq, Time.deltaTime * 10f);
        }
        else if (cb.pm.tempScore < cb.bReq)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, (float)(cb.pm.tempScore - cb.cReq) / (float)(cb.bReq - cb.cReq), Time.deltaTime * 10f);
        }
        else if (cb.pm.tempScore < cb.aReq)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, (float)(cb.pm.tempScore - cb.bReq) / (float)(cb.aReq - cb.bReq), Time.deltaTime * 10f);
        }
        else if (cb.pm.tempScore < cb.sReq)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, (float)(cb.pm.tempScore - cb.aReq) / (float)(cb.sReq - cb.aReq), Time.deltaTime * 10f);
        }
        else
        {
            image.fillAmount = 0f;
        }   
    }
}
