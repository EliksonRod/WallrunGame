using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLevel : MonoBehaviour
{
    int buildindex;
    public int ParticleAmount;
    [SerializeField] GameObject Goal_Barrier;

    [SerializeField] GameObject[] Missing_Particles_UI;
    [SerializeField] GameObject[] Found_Particles_UI;
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
        if (ParticleAmount == 1)
        {
            Missing_Particles_UI[0].SetActive(false);
            Found_Particles_UI[0].SetActive(true);
        }
        if (ParticleAmount == 2)
        {
            Missing_Particles_UI[1].SetActive(false);
            Found_Particles_UI[1].SetActive(true);
        }
        if (ParticleAmount == 3)
        {
            Missing_Particles_UI[2].SetActive(false);
            Found_Particles_UI[2].SetActive(true);
        }

        if (ParticleAmount >= 3)
        {
            Goal_Barrier.SetActive(false);
        }
    }
}
