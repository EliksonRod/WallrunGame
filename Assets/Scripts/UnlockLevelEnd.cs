using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockLevelEnd : MonoBehaviour
{
    [SerializeField] changeLevel GoalScript;
    void OnTriggerEnter(Collider myCollision)
    {
        if (GoalScript != null)
        {
            GoalScript.ParticleAmountNeeded += 1;
            gameObject.SetActive(false);
        }
    }
}
