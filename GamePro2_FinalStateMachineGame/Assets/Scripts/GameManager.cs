using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerMovement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Pause_Menu;
    [SerializeField] GameObject PlayerHUD;
    float originalTimeScale;

    public void Start()
    {
        originalTimeScale = Time.timeScale;
    }

    private void Update()
    {
        if (Pause_Menu != null && Pause_Menu.activeInHierarchy)
        {
            PlayerHUD.SetActive(false);
            StopTime();
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
        PlayerHUD.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = originalTimeScale;
    }
}
