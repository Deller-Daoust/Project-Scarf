using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash_Functions : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    private Shader shaderGUItext, shaderSpritesDefault;

    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
    }

    public void flashSprite() 
    {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
        StartCoroutine(unwhiteSprite());
    }

    public void flashSprite(Color _color) 
    {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = _color;
        StartCoroutine(unwhiteSprite());
    }

    private IEnumerator unwhiteSprite()
    {
        yield return new WaitForSeconds(0.01f);
        myRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        myRenderer.material.shader = shaderSpritesDefault;
    }
}
