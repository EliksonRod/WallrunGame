using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    PlayerMovement playerScript;
    [SerializeField] Animator CheckpointAnim;

    void Awake()
    {
        if (CheckpointAnim != null)
        {
        CheckpointAnim.enabled = false;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckpointAnim != null && other.CompareTag("Player") && !AnimatorIsPlaying())
        {
            CheckpointAnim.enabled = true;
            CheckpointAnim.Play("CheckpointReached", -1, 0.0f);
            playerScript.UpdateCheckpoint(transform.position);
        }
    }

    bool AnimatorIsPlaying()
    {
        return CheckpointAnim.GetCurrentAnimatorStateInfo(0).length > CheckpointAnim.GetCurrentAnimatorStateInfo(0).normalizedTime && CheckpointAnim.GetCurrentAnimatorStateInfo(0).IsName("CheckPointReached");
    }

    //How to make it so that the checkpoint animation plays only once when the player hits the checkpoint, and not every time they hit it again?
    //hitmarker.com
    //workingwithindie.com
}
