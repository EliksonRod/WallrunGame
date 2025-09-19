using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;
    [SerializeField] GameObject UI_Manager;

    bool ButtonPressed = false;
    bool animPlayed = false;

    void OnEnable()
    {
        Button buttonanim = gameObject.GetComponent<Button>();
        animWhenHoverOver.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animWhenHoverOver.enabled = true;
        ButtonPressed = false;
        if (animPlayed != true)
        {
            animWhenHoverOver.enabled = true;
            animWhenHoverOver.Play("Button Pop Out Right");
            animPlayed = true;
        }   
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animPlayed = false;
        animWhenHoverOver.Play("Return Button To Origin");   
    }
    public void returnSize()
    {
        ButtonPressed = true;
        animWhenHoverOver.Play("Return Button To Origin");
        animWhenHoverOver.enabled = false;
    }
}
