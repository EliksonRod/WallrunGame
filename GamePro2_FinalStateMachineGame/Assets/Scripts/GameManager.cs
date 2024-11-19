using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject WallRunMenu;
    //private bool isTimeStopped;
    private float originalTimeScale;
    private void Start()
    {
        originalTimeScale = Time.timeScale;
    }
    // Update is called once per frame
    void Update()
    {
        if(WallRunMenu.activeInHierarchy)
        {
            StopTime();
        }
        else
        {
            ResumeTime();
        }
    }
    private void StopTime()
    {
        //isTimeStopped = true;
        //originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void ResumeTime()
    {
        //isTimeStopped = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = originalTimeScale;
    }
}
