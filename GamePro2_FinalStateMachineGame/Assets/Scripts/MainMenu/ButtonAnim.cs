using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;
    private void Start()
    {
        animWhenHoverOver.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animWhenHoverOver.enabled = true;
        animWhenHoverOver.Play("HoverOverGrowShrink");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animWhenHoverOver.Play("ReturnToNormalSize");   
    }
}
