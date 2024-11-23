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
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Debug.Log("Mouse enter");
        gameObject.GetComponent<Animator>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Debug.Log("Mouse exit");
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
