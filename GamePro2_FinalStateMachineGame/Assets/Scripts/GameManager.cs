using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Would it be better to have stop/resume time on the UIManager Script then GameManager script?

    public GameObject WallRunMenu;
    //private bool isTimeStopped;
    private float originalTimeScale;
    public void Start()
    {
        originalTimeScale = Time.timeScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (WallRunMenu.activeInHierarchy)
        {
            //StopTime();
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
