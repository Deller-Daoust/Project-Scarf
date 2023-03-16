using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options_Controller : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    public void Open()
    {
        if(optionsMenu.activeSelf == false)
        {
            optionsMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        if(optionsMenu.activeSelf == true)
        {
            optionsMenu.SetActive(false);
        }
    }

    public IEnumerator LeftMove()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator RightMove()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator GunAttack()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator ShootScarf()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator Roll()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator Jump()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator Parry()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator Interact()
    {
        yield return new WaitForSeconds(2);
    }

    public IEnumerator SkipDialogue()
    {
        yield return new WaitForSeconds(2);
    }
}
