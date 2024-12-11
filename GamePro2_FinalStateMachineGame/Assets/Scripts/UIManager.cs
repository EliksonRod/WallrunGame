using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject dropdown;
    public GameObject popUpMenu;

private void Awake()
    {
        //audioSource.Play();
        //DontDestroyOnLoad(this.gameObject);
    }
    public void ChooseMode()
    {
        dropdown.SetActive(true);
    }

    public void ExitChooseMode()
    {
        dropdown.SetActive(false);
    }
    public void CloseMenu()
    {
        popUpMenu.SetActive(false);
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

    
}
