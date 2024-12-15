using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrap : MonoBehaviour
{
    public GameObject platform;
    void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
        Destroy(platform);
    }
}
