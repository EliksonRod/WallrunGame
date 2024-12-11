using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public Trap trapScript;

    private void OnTriggerEnter(Collider other)
    {
        trapScript.playerDetected = true;
        Debug.Log(trapScript.playerDetected);
    }
}
