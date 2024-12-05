using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    //Will NOT MOVE at start, activated by player collision, does NOT wait ATSTOP
    //It will loop between destinations (back and forth) as long as player is on it and will pause in its tracks when jumped off/exited

    public SpriteRenderer rend;
    public Collider Coll;

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {

    }

    private void OnCollisionExit(Collision other)
    {

    }

    void turnOffCollider()
    {
        Coll.enabled = false;
    }void turnOnCollider()
    {
        Coll.enabled = true;
    }
    /*public static void ChangeAlpha(this Material mat, float alphaValue)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
        mat.SetColor("_Color", newColor);
    }*/


}
