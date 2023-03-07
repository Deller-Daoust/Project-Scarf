using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SinFloat : MonoBehaviour
{
    public float sin, sinSpeed;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(startPos.x + (Mathf.Sin(sin/1.5f) * 10f), startPos.y + (Mathf.Sin(sin) * 20f), startPos.z);
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(sin/1.3f) * 3f);
        sin += sinSpeed * Time.deltaTime;
    }
}
