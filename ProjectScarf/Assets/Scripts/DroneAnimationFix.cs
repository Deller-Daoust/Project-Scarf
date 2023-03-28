using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimationFix : MonoBehaviour
{
    public float speed = 1f;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
