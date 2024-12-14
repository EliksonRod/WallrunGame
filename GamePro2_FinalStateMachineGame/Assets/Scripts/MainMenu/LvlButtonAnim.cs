using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class LvlButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;

    bool mouseIsOverUI = false;

    private void Start()
    {
        animWhenHoverOver.enabled = false;
    }
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
        animWhenHoverOver.Play("LevelButtons");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOverUI = false;
        animWhenHoverOver.Play("NormalSize");
    }
}
