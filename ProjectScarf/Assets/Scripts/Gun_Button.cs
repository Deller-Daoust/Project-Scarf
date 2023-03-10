using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Button : MonoBehaviour
{
    public GameObject door;
    public int shots = 2;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[shots];
        if (shots == 0)
        {
            Destroy(door);
        }
    }
}
