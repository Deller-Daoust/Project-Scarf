using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Chomp : MonoBehaviour
{
    public ParticleSystem chompPS;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die",1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if (target != null)
        {
            transform.position = new Vector2(target.transform.position.x, target.transform.position.y + (target.GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
