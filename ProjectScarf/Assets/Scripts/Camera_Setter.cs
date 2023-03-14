using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Setter : MonoBehaviour
{
    bool lerpCam;
    public bool followDisable;
    BoxCollider2D box;
    public float lerpSpeed = 5f, zoom = 6.5f;

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D player)
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (followDisable)
            {
                player.gameObject.GetComponent<Player_Movement>().camFollow = false;
            }
            lerpCam = true;
        }
    }

    void OnTriggerExit2D(Collider2D player)
    {
        if (followDisable)
        {
            player.gameObject.GetComponent<Player_Movement>().camFollow = true;
        }
        lerpCam = false;
    }

    void Update()
    {
        if (lerpCam)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom, lerpSpeed * Time.deltaTime);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position, lerpSpeed * Time.deltaTime);
        }
    }
}
