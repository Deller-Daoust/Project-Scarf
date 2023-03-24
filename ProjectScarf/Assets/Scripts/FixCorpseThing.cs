using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCorpseThing : MonoBehaviour
{
    public RectTransform trans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trans.anchoredPosition = Vector2.zero;
    }
}
