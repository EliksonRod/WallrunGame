using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger1: MonoBehaviour
{
    public Trap trapScript;

    private void OnTriggerEnter(Collider other)
    {
        trapScript.playerDetected = true;
    }
}
