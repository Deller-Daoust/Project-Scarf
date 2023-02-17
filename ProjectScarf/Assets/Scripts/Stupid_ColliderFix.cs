using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stupid_ColliderFix : MonoBehaviour
{
    // Start is called before the first frame update

    //FIX THIS!!!!!!! i hate my life
    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Vector3 v = GetComponent<Renderer>().bounds.size; 
        BoxCollider2D b = GetComponent<Collider2D>() as BoxCollider2D;
        b.size = v;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(spriteRenderer.sprite.rect.width / 2, -4);
        Debug.Log(spriteRenderer.sprite.rect.width);
    }
}
