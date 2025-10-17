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
    




    // Update is called once per frame
    void Update()
    {
        /*overGround = Physics.Raycast(transform.position, Vector3.down, 2f + 0.2f, whatIsGround);

        TeleportSkill();
        AbilityCooldownManager();

        //Records last position on ground for return teleport ability
        if (overGround)
            positionBeforeTeleport = gameObject.transform.position;

        //Pause menu and teleport timescale don't mess with each other
        if (pauseMenu.activeInHierarchy == false /*&& playerState == PlayerState.teleporting)
        {
            Time.timeScale = 0.2f;
            TeleportTargetIndicator.SetActive(true);
        }
        else if (pauseMenu.activeInHierarchy == false/* && playerState != PlayerState.teleporting)
        {
            Time.timeScale = 1f;
            TeleportTargetIndicator.SetActive(false);
        }*/
    }

   
}
