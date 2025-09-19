using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public SecretWall trapScript;
    bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used == false)
        {
            trapScript.playerDetected = true;
            used = true;
        }
    }
}
