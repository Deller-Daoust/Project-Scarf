using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarf_Behaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Combat_System combatSys;

    private Collision2D collision;
    private Vector3 finalPos;

    private float LerpTime = 0.5f;

    void OnCollisionEnter2D(Collision2D col)
    {
        finalPos = new Vector3(col.transform.position.x - 1.5f, col.transform.position.y + 0.17f, col.transform.position.z);

        if(col.gameObject.tag == "Enemy")
        {
            StopCoroutine(combatSys._scarfOut);
            StartCoroutine(combatSys.ScarfIn());
            
            StartCoroutine(ScarfPull());
        }
    }

    IEnumerator ScarfPull()
    {
        float startTime = Time.time;
        float EndTime = startTime + LerpTime;

        while(Time.time < EndTime)
        {
            float timeProg = (Time.time - startTime) / LerpTime;
            player.transform.position = Vector3.Lerp(player.transform.position, finalPos, timeProg / 8);
            //player.GetComponent<Player_Movement>().body.AddForce(Vector2.up * 1.5f, ForceMode2D.Impulse);  Leap towards Enemy test.

            yield return new WaitForEndOfFrame();
        }

        combatSys.scarf.SetActive(false);
    }
}
