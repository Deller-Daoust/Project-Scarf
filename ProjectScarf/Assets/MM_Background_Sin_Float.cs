using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Background_Sin_Float : MonoBehaviour
{
    private float sin;
    public float sinOffset;
    public bool randomOffset;
    public float sinSpeed = 1.2f;
    public bool floatX = false;
    public float xFactor = 1.5f, xOffset = 0.5f;
    public bool floatY = true;
    public float yFactor = 1f, yOffset = 1f;
    private Vector3 startPos;
    public bool floatRotate;
    public float rotFactor = 1.3f, rotOffset = 3f;
    // Start is called before the first frame update
    void Start()
    {
        if (randomOffset)
        {
            sinOffset = Random.Range(0f,10000f);
        }
        sin = sinOffset;
        startPos = GetComponent<RectTransform>().anchoredPosition;
        if (!floatX)
        {
            xOffset = 0f;
        }
        if (!floatY)
        {
            yOffset = 0f;
        }
    }

    // Update is called once per frame test
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(startPos.x + (Mathf.Sin(sin/xFactor) * xOffset), startPos.y + (Mathf.Sin(sin/yFactor) * yOffset), startPos.z);
        if (floatRotate)
        {
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(sin/rotFactor) * rotOffset);
        }
        sin += sinSpeed * Time.deltaTime;
    }
}
