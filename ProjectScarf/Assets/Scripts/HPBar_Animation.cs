using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar_Animation : MonoBehaviour
{
    public Animator anim;
    
    public void PlayReverse()
    {
        anim.Play("HP_FlyOut");
    }
}
