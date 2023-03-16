using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Hook_Handler : MonoBehaviour
{
    public Animator anim;

    public void GoUp()
    {
        anim.Play("float");
    }

    public void GoDown()
    {
        anim.Play("unfloat");
    }
}
