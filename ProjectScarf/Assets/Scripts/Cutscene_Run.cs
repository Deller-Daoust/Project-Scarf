using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Run : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 149f)
        {
            GetComponent<Door>().gotoScene();
        }
    }
}
