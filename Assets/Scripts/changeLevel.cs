using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeLevel : MonoBehaviour
{
    int buildindex;
    
    [SerializeField] bool LevelUsesBarrier;
    [SerializeField] GameObject GoalBarrier;
    public string SceneName;

    [HideInInspector] public int ParticleAmountNeeded;
    [SerializeField] GameObject[] Missing_Particles_UI;
    [SerializeField] GameObject[] Found_Particles_UI;
    void Start()
    {
        buildindex = SceneManager.GetActiveScene().buildIndex;
    }

    void OnTriggerEnter(Collider myCollision)
    {
        if (ParticleAmountNeeded >= 3 || !LevelUsesBarrier)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
    void Update()
    {
        //ParticleAmountNeeded = PlayerPrefs.GetInt("ParticlesCollected_" + buildindex, 0);
        ParticleUI();
    }

    void ParticleUI()
    {
        if (!LevelUsesBarrier) return;

        if (ParticleAmountNeeded == 1)
        {
            if (Missing_Particles_UI != null && Found_Particles_UI != null)
            {
                Missing_Particles_UI[0].SetActive(false);
                Found_Particles_UI[0].SetActive(true);
            }
        }
        if (ParticleAmountNeeded == 2)
        {
            if (Missing_Particles_UI != null && Found_Particles_UI != null)
            {
                Missing_Particles_UI[1].SetActive(false);
                Found_Particles_UI[1].SetActive(true);
            }
        }
        if (ParticleAmountNeeded == 3)
        {
            if (Missing_Particles_UI != null && Found_Particles_UI != null)
            {
                Missing_Particles_UI[2].SetActive(false);
                Found_Particles_UI[2].SetActive(true);
            }
        }

        if (ParticleAmountNeeded >= 3)
        {
            if (GoalBarrier != null)
                GoalBarrier.SetActive(false);
        }
    }
}
