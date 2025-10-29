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

public class Dashing : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;
    public RectTransform[] DashCounters;
    public GameObject PauseMenu;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("CameraEffects")]
    public playerCam cam;
    public float dashFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    float barWidth;

    int numberOfDashes = 3;
    public float TimeBeforeDashRecharge = 1f;
    public float dashCooldown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        for (int i = 0; i < DashCounters.Length; i++)
        {
            barWidth = DashCounters[i].anchorMax.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(dashKey))
            Dash();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
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
    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        cam.DoFov(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam; /// where you're looking
        else
            forwardT = orientation; /// where you're facing (no up or down)

        //Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce ;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        //DelayedDashForce();
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }
    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.linearVelocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0;

        cam.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
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
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, barWidth);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, barWidth);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes == 2)
        {
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, 0f);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, barWidth);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes == 1)
        {
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, 0f);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, 0f);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes <= 0)
        {
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, 0f);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, 0f);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, 0f);
        }
    }
}
