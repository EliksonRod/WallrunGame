using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLevel : MonoBehaviour
{
    int buildindex;
    //public string sceneName; 

    void Start()
    {
        buildindex = SceneManager.GetActiveScene().buildIndex;
    }

    void OnTriggerEnter(Collider myCollision)
    {
        SceneManager.LoadScene(buildindex + 1);
        //SceneManager.LoadScene(sceneName);
    }
}
