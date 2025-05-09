using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLevel : MonoBehaviour
{
    int buildindex;
    public static int ParticleAmount;
    [SerializeField] GameObject Goal_Barrier;
    void Start()
    {
        buildindex = SceneManager.GetActiveScene().buildIndex;
    }

    void OnTriggerEnter(Collider myCollision)
    {
        if (ParticleAmount >= 3)
        {
            SceneManager.LoadScene(buildindex + 1);
        }
    }
    void Update()
    {
        if (ParticleAmount >= 3)
        {
            Goal_Barrier.SetActive(false);
        }
    }
}
