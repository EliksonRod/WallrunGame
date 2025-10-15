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

public class AbilityManager : MonoBehaviour
{

    [Header("Timers")]
    public float TeleportCooldown = 5f, TeleportTimer;
    public float ReturnCooldown = 4f, ReturnTimer;

    [Header("Teleportation")]
    public RawImage cursor;
    UnityEngine.Color defaultColor;
    public UnityEngine.Color teleportColor;
    Vector3 positionBeforeTeleport;
    public int numberOfTeleports;
    public float teleportDistance;
    public bool canTeleport = true, canReturnTeleport = true;
    public bool teleportTarget = false;
    public GameObject TeleportTargetIndicator;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public bool overGround;

    [Header("References")]
    public GameObject pauseMenu;
    Rigidbody rb;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultColor = cursor.color;
        teleportColor.a = 1;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        overGround = Physics.Raycast(transform.position, Vector3.down, 2f + 0.2f, whatIsGround);

        TeleportSkill();
        AbilityCooldownManager();

        //Records last position on ground for return teleport ability
        if (overGround)
            positionBeforeTeleport = gameObject.transform.position;

        //Pause menu and teleport timescale don't mess with each other
        if (pauseMenu.activeInHierarchy == false /*&& playerState == PlayerState.teleporting*/)
        {
            Time.timeScale = 0.2f;
            TeleportTargetIndicator.SetActive(true);
        }
        else if (pauseMenu.activeInHierarchy == false/* && playerState != PlayerState.teleporting*/)
        {
            Time.timeScale = 1f;
            TeleportTargetIndicator.SetActive(false);
        }
    }

        void TeleportSkill()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (TeleportTargetIndicator != null)
            {
                TeleportTargetIndicator.transform.position = hit.point;
            }

            if (Physics.Raycast(ray, out hit, teleportDistance, whatIsGround))
            {
                cursor.color = teleportColor;

                //LMB to teleport
                if (Input.GetKeyDown(KeyCode.Mouse0) && canTeleport == true)
                {
                    canTeleport = false;
                    TeleportTimer = TeleportCooldown;
                    numberOfTeleports -= 1;

                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                    transform.position = hit.point;
                    //playerState = PlayerState.None;
                }
            }
            else
            {
                cursor.color = defaultColor;
            }

            //RMB to teleport to position before teleport
            if (Input.GetKeyDown(KeyCode.Mouse1) && canReturnTeleport == true)
            {
                canReturnTeleport = false;
                ReturnTimer = ReturnCooldown;
                gameObject.transform.position = positionBeforeTeleport;
                //playerState = PlayerState.None;
            }
        }
    }

    void AbilityCooldownManager()
    {
        if (canTeleport == false)
        {
            TeleportTimer -= Time.deltaTime;

            if (TeleportTimer <= 0f)
            {
                canTeleport = true;
            }
        }

        if (canReturnTeleport == false)
        {
            ReturnTimer -= Time.deltaTime;

            if (ReturnTimer <= 0f)
            {
                canReturnTeleport = true;
            }
        }
    }
}
