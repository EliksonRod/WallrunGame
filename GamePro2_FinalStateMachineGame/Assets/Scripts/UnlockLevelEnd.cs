using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockLevelEnd : MonoBehaviour
{
    [SerializeField] changeLevel GoalScript;
    void OnTriggerEnter(Collider myCollision)
    {
        GoalScript.ParticleAmount += 1;
        gameObject.SetActive(false);
    }
}
