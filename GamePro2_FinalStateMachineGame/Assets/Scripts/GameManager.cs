using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject WallRunMenu;
    public GameObject MovementMenu;
    public GameObject ClimbingMenu;
    public GameObject PauseMenu;
    //GameObject[] menus;
    float originalTimeScale;

    public void Start()
    {
        originalTimeScale = Time.timeScale;
    }
    void Update()
    {
        /*menus = GameObject.FindGameObjectsWithTag("Menu");
        if (menus.Length > 0)
        {
            foreach (GameObject menu in menus)
            {
                if (menu.activeInHierarchy)
                {
                    StopTime();
                    return;
                }
            }
        }
        else 
        {
            ResumeTime();
        }  */         

        if ((WallRunMenu != null && WallRunMenu.activeInHierarchy) || (ClimbingMenu != null && ClimbingMenu.activeInHierarchy) || (MovementMenu != null && MovementMenu.activeInHierarchy))
        {
            //StopTime();
        }
        else if (PauseMenu.activeInHierarchy)
        {
            StopTime();
        }
        else
        {
            ResumeTime();
        }
    }
    public void StopTime()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeTime()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = originalTimeScale;
    }
}
