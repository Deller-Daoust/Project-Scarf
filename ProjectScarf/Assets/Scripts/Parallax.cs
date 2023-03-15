using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    //GameObject player;
    public float levelLeft = 0f, levelRight, offset = 280f;
    public Vector2 playerPos;
    RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        //player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = new Vector2((levelLeft + Camera.main.transform.position.x) / levelRight, Camera.main.transform.position.y);
        playerPos = new Vector2((playerPos.x - 0.5f) * 2f, (playerPos.y - 0.5f) * 2f);
        rt.anchoredPosition = new Vector2(offset * -playerPos.x, 0f);
    }
}
