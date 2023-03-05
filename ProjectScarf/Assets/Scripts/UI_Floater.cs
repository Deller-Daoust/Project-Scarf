using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Floater : MonoBehaviour
{
    public float offset = 48f, hoverSpeed = 10f;
    private float startY;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
        startY = transform.position.y;
    }

    void Update()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(0f, 0.7f), hoverSpeed * Time.deltaTime);
        GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0), (hoverSpeed / 2) * Time.deltaTime);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
