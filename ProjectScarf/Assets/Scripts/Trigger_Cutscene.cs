using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Cutscene : MonoBehaviour
{
    bool lerpCam;
    private Vector3 targetPos;
    public GameObject ui;
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.gameObject.GetComponent<Player_Movement>().canInput = false;
            player.gameObject.GetComponent<Player_Movement>().musicSource.Stop();
            player.gameObject.GetComponent<Player_Movement>().camFollow = false;
            lerpCam = true;
            player.gameObject.GetComponent<Character_RunIn>().enabled = true;
        }
    }

    void Update()
    {
        if (lerpCam)
        {
            ui.SetActive(false);
            targetPos = new Vector3(148f, 6f, -1f);
            Debug.Log(targetPos);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, 5f * Time.deltaTime);
        }
    }
}
