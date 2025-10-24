using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public AudioSource audioSource;

    public GameObject[] PreviousUI;
    public GameObject[] NextUI;
    [SerializeField] PlayerMovement playerScript;
    [SerializeField] GameManager gameManager;

    // Switch UIs
    public void SwitchUI()
    {
        if (PreviousUI != null)
        {
            //Disable all animators at start
            for (int i = 0; i < PreviousUI.Length; i++)
                PreviousUI[i].SetActive(false);
        }

        if (NextUI != null)
        {
            //Disable all animators at start
            for (int i = 0; i < NextUI.Length; i++)
                NextUI[i].SetActive(true);
        }
    }


    //Main Menu Functions
    public void PlayNextScene()
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

    //Pause Menu Functions
    public void ClosePauseUI()
    {
        gameManager.ResumeTime();

        if (PreviousUI != null)
        {
            //Close all previous UIs
            for (int i = 0; i < PreviousUI.Length; i++)
                PreviousUI[i].SetActive(false);
        }
    }
    public void ResetToCheckpoint()
    {
        playerScript.RespawnPlayer();
        gameManager.ResumeTime();

        if (PreviousUI != null)
        {
            //Close all previous UIs
            for (int i = 0; i < PreviousUI.Length; i++)
                PreviousUI[i].SetActive(false);
        }
    }
    public void ReloadScene()
    {
        gameManager.ResumeTime();

        if (PreviousUI != null)
        {
            //Close all previous UIs
            for (int i = 0; i < PreviousUI.Length; i++)
                PreviousUI[i].SetActive(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }   


}
