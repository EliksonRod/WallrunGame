using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Unity.Burst;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Rendering;
using DG.Tweening.Core.Easing;
using System.Drawing.Drawing2D;
using System.Diagnostics.Eventing.Reader;

public class AbilityManager : MonoBehaviour
{
    public RectTransform[] Meters;
    public PlayerMovement pm;
    public GameObject PauseMenu;

    float barWidth;

    int numberOfDashes = 3;
    public float TimeBeforeDashRecharge = 1f;
    public float dashCooldown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        //barWidth = Meters[0].anchorMax.x;

        for (int i = 0; i < Meters.Length; i++)
        {
            barWidth = Meters[i].anchorMax.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TeleportMeter();
        DashUI();
        AbilityInputs();
    }

    void AbilityInputs()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)/* && !pm.grounded && !pm.wallrunning && numberOfDashes > 0*/)
        {
            DashAbility();
        }
    }

    void DashAbility()
    {
        // Dash Ability
        numberOfDashes -= 1;


        StopAllCoroutines();
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(TimeBeforeDashRecharge);
        yield return new WaitForSeconds(dashCooldown);

        if (numberOfDashes < 3)
        {
            numberOfDashes += 1;
        }

        yield return new WaitForSeconds(dashCooldown);

        if (numberOfDashes < 3)
        {
            numberOfDashes += 1;
        }

        yield return new WaitForSeconds(dashCooldown);

        if (numberOfDashes < 3)
        {
            numberOfDashes += 1;
        }
    }

    void DashUI()
    {
        /*dashTimer = ((dashTimer > dashCooldown) ? dashCooldown : (dashTimer < 0) ? 0 : dashTimer);

        float y = ((dashTimer * (100f / dashCooldown)) * (1f / barWidth)) / 100f;

        Meters[MeterToRecover].anchorMax = new Vector2(-Meters[MeterToRecover].anchorMax.x, y);*/

        if (numberOfDashes == 3)
        {
            Meters[0].anchorMax = new Vector2(Meters[0].anchorMax.x, barWidth);
            Meters[1].anchorMax = new Vector2(Meters[1].anchorMax.x, barWidth);
            Meters[2].anchorMax = new Vector2(Meters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes == 2)
        {
            Meters[0].anchorMax = new Vector2(Meters[0].anchorMax.x, 0f);
            Meters[1].anchorMax = new Vector2(Meters[1].anchorMax.x, barWidth);
            Meters[2].anchorMax = new Vector2(Meters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes == 1)
        {
            Meters[0].anchorMax = new Vector2(Meters[0].anchorMax.x, 0f);
            Meters[1].anchorMax = new Vector2(Meters[1].anchorMax.x, 0f);
            Meters[2].anchorMax = new Vector2(Meters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes <= 0)
        {
            Meters[0].anchorMax = new Vector2(Meters[0].anchorMax.x, 0f);
            Meters[1].anchorMax = new Vector2(Meters[1].anchorMax.x, 0f);
            Meters[2].anchorMax = new Vector2(Meters[2].anchorMax.x, 0f);
        }
    }
}
