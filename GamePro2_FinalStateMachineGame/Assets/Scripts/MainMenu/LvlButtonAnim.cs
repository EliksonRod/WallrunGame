using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class LvlButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animWhenHoverOver;
    public GameObject levelMenu;
    bool menuIsActive = false;
    public GameObject levelCanvas;

    bool mouseIsOverUI = false;

    private void Start()
    {
        animWhenHoverOver.enabled = false;
    }
    private void Update()
    {
        if (mouseIsOverUI == false && menuIsActive == false)
        {
            //animWhenHoverOver.enabled = false;
            animWhenHoverOver.Play("NormalSize");
        }

        if (levelCanvas.activeInHierarchy){}
        else 
        {
            mouseIsOverUI = false;
            menuIsActive = false;
            //animWhenHoverOver.enabled = false;
            animWhenHoverOver.Play("NormalSize");
            Debug.Log("mainMenu");
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
        if (levelMenu.activeInHierarchy)
        {
            menuIsActive = true;
            animWhenHoverOver.Play("LevelButtons");
        }
        else
        {
            mouseIsOverUI = false;
            animWhenHoverOver.Play("NormalSize");
        }
    }
}
