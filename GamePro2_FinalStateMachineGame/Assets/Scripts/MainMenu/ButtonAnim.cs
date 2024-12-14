using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;

    bool mouseIsOverUI = false;
    private void FixedUpdate()
    {
        if (mouseIsOverUI == false)
        {
            animWhenHoverOver.enabled = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOverUI = true;
        animWhenHoverOver.enabled = true;
        animWhenHoverOver.Play("HoverOverGrowShrink");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOverUI = false;
        animWhenHoverOver.Play("ReturnToNormalSize");   
    }
    public void returnSize()
    {
        mouseIsOverUI = false;
        animWhenHoverOver.Play("ReturnToNormalSize");
    }
}
