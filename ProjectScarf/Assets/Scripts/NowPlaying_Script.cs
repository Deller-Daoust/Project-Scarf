using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowPlaying_Script : MonoBehaviour
{
    Vector2 startPosition;
    public float offset = 100f, speed = 1f;
    private RectTransform trans;
    private bool approaching = true, depproaching;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<RectTransform>();
        startPosition = trans.anchoredPosition;
        trans.anchoredPosition = new Vector2(trans.anchoredPosition.x + offset, trans.anchoredPosition.y);
        StartCoroutine(SwapApproach());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (approaching && trans.anchoredPosition.x > startPosition.x)
        {
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x - speed, trans.anchoredPosition.y);
        }
        if (depproaching)
        {
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x + speed, trans.anchoredPosition.y);
        }
    }

    IEnumerator SwapApproach()
    {
        yield return new WaitForSeconds(5f);
        approaching = false;
        depproaching = true;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
