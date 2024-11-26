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
        gameObject.GetComponent<Animator>().enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Animator>().enabled = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

}
