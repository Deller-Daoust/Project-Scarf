using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Hook : MonoBehaviour
{
    public Transform target;
    public float pos;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector2(target.localPosition.x * pos, target.localPosition.y * pos);
    }
}
