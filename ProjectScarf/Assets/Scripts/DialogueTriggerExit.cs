using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerExit : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == transform.parent.GetComponent<Dialogue_Trigger>().player)
        {
            transform.parent.GetComponent<Dialogue_Trigger>().dialoguebox.SetActive(false);
        }
    }
}
