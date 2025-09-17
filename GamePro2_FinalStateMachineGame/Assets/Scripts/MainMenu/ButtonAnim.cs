using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;


public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;

    bool mouseIsOverUI = false;
    bool animPlayed = false;

    void Start()
    {
        animWhenHoverOver.enabled = false;
    }
    private void FixedUpdate()
    {
        if (mouseIsOverUI == false)
        {
            //animWhenHoverOver.enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animWhenHoverOver.enabled = true;
        mouseIsOverUI = true;

        if (animPlayed != true)
        {
            animWhenHoverOver.enabled = true;
            animWhenHoverOver.Play("HoverOverGrowShrink");
            animPlayed = true;
        }   
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOverUI = false;
        animPlayed = false;
        animWhenHoverOver.Play("ReturnToNormalSize");   
    }
    public void returnSize()
    {
        mouseIsOverUI = false;
        animWhenHoverOver.Play("ReturnToNormalSize");
    }
}
