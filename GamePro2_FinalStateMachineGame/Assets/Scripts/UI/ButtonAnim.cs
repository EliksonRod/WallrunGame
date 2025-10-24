using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator Sweeping_UI_Anim;
    public Animator GrowShrink_UI_Anim;
    bool mouseIsOverUI = false;

    [SerializeField] bool UseSwipeAnimation;
    [SerializeField] bool UseGrowShrinkAnimation;

    void OnEnable()
    {
        //Set swipe animation to default position on enable
        if (UseSwipeAnimation && Sweeping_UI_Anim != null)
            Sweeping_UI_Anim.Play("Default Position");

        //Set grow/shrink animation to normal size on enable
        if (UseGrowShrinkAnimation && GrowShrink_UI_Anim != null)
            GrowShrink_UI_Anim.Play("Normal Size");
    }

    void FixedUpdate()
    {
        //Return to normal size if mouse is not over UI and using grow/shrink animation
        if (mouseIsOverUI == false && GrowShrink_UI_Anim != null && UseGrowShrinkAnimation)
        {
            GrowShrink_UI_Anim.Play("NormalSize");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Play swipe animation on pointer enter
        if (UseSwipeAnimation && Sweeping_UI_Anim != null)
            Sweeping_UI_Anim.Play("Button Pop Out Right");

        //Play grow/shrink animation on pointer enter
        if (UseGrowShrinkAnimation && GrowShrink_UI_Anim != null)
        {
            mouseIsOverUI = true;
            GrowShrink_UI_Anim.Play("LevelButtons");
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Return swipe animation to origin on pointer exit
        if (UseSwipeAnimation && Sweeping_UI_Anim != null)
            Sweeping_UI_Anim.Play("Return Button To Origin");

        //Return grow/shrink animation to normal size on pointer exit
        if (UseGrowShrinkAnimation && GrowShrink_UI_Anim != null)
        {
            GrowShrink_UI_Anim.Play("NormalSize");
            mouseIsOverUI = false;
        }
    }
    // Function to return swipe animation to default position when button is pressed
    public void returnSize()
    {
        Sweeping_UI_Anim.Play("Default Position");
    }
}
