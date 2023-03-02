using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScarf_Sprite : MonoBehaviour
{
    [SerializeField] private Hook_Behaviour hookScarfBehav;

    private Vector3 targetScale;

    // Start is called before the first frame update
    void Start()
    {
        hookScarfBehav = gameObject.GetComponentInParent<Hook_Behaviour>();

        targetScale = new Vector3(hookScarfBehav.distance, hookScarfBehav.distance, 0);
        StartCoroutine(ScaleGrow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScaleGrow()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / 2000);

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
