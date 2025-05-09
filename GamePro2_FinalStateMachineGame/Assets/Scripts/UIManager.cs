using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject UI_Menu;
    [SerializeField] playerMovement playerScript;

private void Awake()
    {
        //audioSource.Play();
        //DontDestroyOnLoad(this.gameObject);
    }
    public void OpenUI()
    {
        UI_Menu.SetActive(true);
    }

    public void CloseUI()
    {
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
