using UnityEngine;
using System.Collections;

public class MirrorRotate : MonoBehaviour
{
    [SerializeField] Quaternion[] rotations;
    [Tooltip("Sets target for next rotation.")]
    [SerializeField] int queuedRotation = 1;
    bool rotationInProgress;

    void Update()
    {
            
    }

    IEnumerator Turn(Quaternion target)
    {
        rotationInProgress = true;

        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < 1.0f)
        {
            transform.rotation = Quaternion.Lerp(startRotation, target, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = target;

        if (queuedRotation < rotations.Length - 1) queuedRotation++;
        else queuedRotation = 0;

        rotationInProgress = false;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") && !rotationInProgress)
        {
            StartCoroutine(Turn(rotations[queuedRotation]));
        }
    }
}
