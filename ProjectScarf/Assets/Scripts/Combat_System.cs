using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_System : MonoBehaviour
{
    [SerializeField] private GameObject scarf;
    private Vector3 targetScale;
    private Vector3 baseScale;

    private Vector3 startPos;
    private Vector3 targetPos;

    private float LerpTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        targetScale = new Vector3(2, 0.1f, 0);

        baseScale = scarf.transform.localScale;

        startPos = scarf.transform.localPosition;
        targetPos = new Vector3(1, startPos.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            scarf.SetActive(true);
            StartCoroutine(Scarf());
        }
    }

    IEnumerator Scarf()
    {
        float startTime = Time.time;
        float EndTime = startTime + LerpTime;

        while(Time.time < EndTime)
        {
            float timeProg = (Time.time - startTime) / LerpTime;
            scarf.transform.localScale = Vector3.Lerp(baseScale, targetScale, timeProg * 7);
            scarf.transform.localPosition = Vector3.Lerp(startPos, targetPos, timeProg * 7);

            yield return new WaitForFixedUpdate();
        }
    }
}
