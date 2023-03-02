using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook_Behaviour : MonoBehaviour
{
    [SerializeField] private Vector2 hookCheck;

    [SerializeField] LayerMask hookLayer;

    private Collider2D[] hookColliders;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hookColliders = Physics2D.OverlapBoxAll(transform.position, hookCheck, hookLayer);

        Debug.Log(hookColliders);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, hookCheck);
    }
}
