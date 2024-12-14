using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlSelectUIManager : MonoBehaviour
{
    public GameObject MainMenuCanvas; 
    public GameObject LvlSelectCanvas;
    public GameObject SettingsCanvas;

    public GameObject defaultMenu;
    public GameObject tutorialMenu;
    public GameObject shoddyMenu;
    public GameObject jumpyMenu;
    public GameObject hugWallMenu;
    public GameObject clockWorkMenu;
    public GameObject dawningMenu;

    public void OpenLevelMenu()
    {
        MainMenuCanvas.SetActive(false);
        LvlSelectCanvas.SetActive(true);
        defaultMenu.SetActive(true);
    }
    public void OpenMainMenu()
    {
        MainMenuCanvas.SetActive(true);
        LvlSelectCanvas.SetActive(false);
        tutorialMenu.SetActive(false);
        shoddyMenu.SetActive(false);
        jumpyMenu.SetActive(false);
        hugWallMenu.SetActive(false);
        clockWorkMenu.SetActive(false);
        dawningMenu.SetActive(false);
    }

    public void TutorialSelect()
    {
        defaultMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        shoddyMenu.SetActive(false);
        jumpyMenu.SetActive(false);
        hugWallMenu.SetActive(false);
        clockWorkMenu.SetActive(false);
        dawningMenu.SetActive(false);
    }
    public void ShoddySelect()
    {
        defaultMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        shoddyMenu.SetActive(true);
        jumpyMenu.SetActive(false);
        hugWallMenu.SetActive(false);
        clockWorkMenu.SetActive(false);
        dawningMenu.SetActive(false);
    }
    public void JumpySelect()
    {
        defaultMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        shoddyMenu.SetActive(false);
        jumpyMenu.SetActive(true);
        hugWallMenu.SetActive(false);
        clockWorkMenu.SetActive(false);
        dawningMenu.SetActive(false);
    }
    public void HugWallSelect()
    {
        defaultMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        shoddyMenu.SetActive(false);
        jumpyMenu.SetActive(false);
        hugWallMenu.SetActive(true);
        clockWorkMenu.SetActive(false);
        dawningMenu.SetActive(false);
    }
    public void ClockWorkSelect()
    {
        defaultMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        shoddyMenu.SetActive(false);
        jumpyMenu.SetActive(false);
        hugWallMenu.SetActive(false);
        clockWorkMenu.SetActive(true);
        dawningMenu.SetActive(false);
    }
    public void DawningSelect()
    {
        defaultMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        shoddyMenu.SetActive(false);
        jumpyMenu.SetActive(false);
        hugWallMenu.SetActive(false);
        clockWorkMenu.SetActive(false);
        dawningMenu.SetActive(true);
    }
    public void OpenSettings()
    {
        MainMenuCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }
    public void CloseSettings()
    {
        MainMenuCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
    }
}
