using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject UI_Menu;
    [SerializeField] playerMovement playerScript;
    [SerializeField] GameManager gameManager;

    public void OpenUI()
    {
        UI_Menu.SetActive(true);
    }

    public void CloseUI()
    {
        gameManager.ResumeTime();
        UI_Menu.SetActive(false);
    }

    public void PlayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void ResetToCheckpoint()
    {
        playerScript.RespawnPlayer();
        UI_Menu.SetActive(false);
    }

    
}
