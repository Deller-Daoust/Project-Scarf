using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinFloat : MonoBehaviour
{
    private float sin;
    public float sinOffset;
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
        sin = sinOffset;
        startPos = transform.position;
        if (!floatX)
        {
            xOffset = 0f;
        }
        if (!floatY)
        {
            yOffset = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(startPos.x + (Mathf.Sin(sin/xFactor) * xOffset), startPos.y + (Mathf.Sin(sin/yFactor) * yOffset), startPos.z);
        if (floatRotate)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(sin/rotFactor) * rotOffset);
        }
        sin += sinSpeed * Time.deltaTime;
    }
}
