using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    playerMovement playerScript;
    [SerializeField] Animator animator;

    void Awake()
    {
        animator.enabled = false;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovement>();
    }

    //hitmarker.com
    //workingwithindie.com
}
