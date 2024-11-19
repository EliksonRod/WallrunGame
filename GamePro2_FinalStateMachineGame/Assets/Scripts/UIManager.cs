using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject dropdown;

    private void Start()
    {
        //dropdown.SetActive(false);
    }

    public void PlayScene()
    {
        SceneManager.LoadScene(1);
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
        dropdown.SetActive(false);
    }
}
