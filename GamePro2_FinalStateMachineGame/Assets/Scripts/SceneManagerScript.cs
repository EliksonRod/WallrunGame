using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private void Awake()
    {
        audioSource.Play();
        DontDestroyOnLoad(this.gameObject);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    
    
   
}
