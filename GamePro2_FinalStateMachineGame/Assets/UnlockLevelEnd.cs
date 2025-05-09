using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockLevelEnd : MonoBehaviour
{
    void OnTriggerEnter(Collider myCollision)
    {
        changeLevel.ParticleAmount += 1;
        gameObject.SetActive(false);
    }
}
