using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;

    void OnEnable()
    {
        animWhenHoverOver.Play("Default Position");
        Button buttonanim = gameObject.GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animWhenHoverOver.Play("Button Pop Out Right");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animWhenHoverOver.Play("Return Button To Origin");   
    }
    public void returnSize()
    {
        animWhenHoverOver.Play("Default Position");
    }
}
