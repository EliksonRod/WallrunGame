using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;
    private bool mouse_over = false;


    private void Start()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }
    private void Update()
    {
        if (mouse_over)
        {
           // Debug.Log("Mouse Over");
            //animWhenHoverOver.Play("HoverOverGrowShrink");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Debug.Log("Mouse enter");
        //animWhenHoverOver.Play("HoverOverGrowShrink");
        gameObject.GetComponent<Animator>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Debug.Log("Mouse exit");
        //animWhenHoverOver.enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;



    }
}
