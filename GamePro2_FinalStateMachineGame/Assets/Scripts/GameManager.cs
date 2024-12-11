using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Would it be better to have stop/resume time on the UIManager Script then GameManager script?

    public GameObject WallRunMenu;
    public GameObject MovementMenu;
    public GameObject ClimbingMenu;
    public GameObject PauseMenu;
    //private bool isTimeStopped;
    private float originalTimeScale;
    public void Start()
    {
        originalTimeScale = Time.timeScale;
    }
    // Update is called once per frame
    void Update()
    {
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
        //isTimeStopped = true;
        //originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeTime()
    {
        //isTimeStopped = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = originalTimeScale;
    }
}
