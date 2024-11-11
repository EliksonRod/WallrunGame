using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject targetPlayer;
    public GameObject targetBall;
    public float mySpeed = 1f;
    public NPC myScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPlayer = GameObject.FindWithTag("Player");
        targetBall = GameObject.FindWithTag("Ball");
        myScript = GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update not defined yet");
    }
    /*protected override void Move()
    {
        Debug.Log("Move is not defined for this object: " + name);
    }
    protected override void Kick()
    {
        Debug.Log("Move is not defined for this object: " + name);
    }
    protected override void Jump()
    {
        Debug.Log("Move is not defined for this object: " + name);
    }*/
}
