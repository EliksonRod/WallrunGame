using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public AudioSource audioSource;

    public GameObject[] CloseUI;
    public GameObject[] OpenUI;
    [SerializeField] PlayerMovement playerScript;
    public static UIManager instance;
    GameManager gm;

    private void Awake()
    {
        gm = GameManager.instance;
    }

    // Switch UIs
    public void SwitchUI()
    {
        if (CloseUI != null)
        {
            //Disable all animators at start
            for (int i = 0; i < CloseUI.Length; i++)
                CloseUI[i].SetActive(false);
        }

        if (OpenUI != null)
        {
            //Disable all animators at start
            for (int i = 0; i < OpenUI.Length; i++)
                OpenUI[i].SetActive(true);
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
        gm.NormalTime();
        gm.TurnOnNormalUI();
        gm.currentGameState = GameManager.GameState.Normal;   

        if (CloseUI != null)
        {
            //Close all previous UIs
            for (int i = 0; i < CloseUI.Length; i++)
                CloseUI[i].SetActive(false);
        }
    }
    public void ResetToCheckpoint()
    {
        playerScript.Respawn();
        gm.NormalTime();

        if (CloseUI != null)
        {
            //Close all previous UIs
            for (int i = 0; i < CloseUI.Length; i++)
                CloseUI[i].SetActive(false);
        }
    }
    public void ReloadScene()
    {
        gm.NormalTime();

        if (CloseUI != null)
        {
            //Close all previous UIs
            for (int i = 0; i < CloseUI.Length; i++)
                CloseUI[i].SetActive(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }   


}
