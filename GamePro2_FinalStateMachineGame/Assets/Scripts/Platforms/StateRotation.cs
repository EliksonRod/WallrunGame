using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StateRotation : MonoBehaviour
{
    public float rotateX = 100f;
    public float rotateY = 100f;
    public float rotateZ = 100f;
    private Transform itemTransform;
    public float timer = 3;
    private bool playerIsOn = false;
    public enum rotateMode
    {
        IDLE,
        ROTATING,
        PAUSE
    }
    private void Start()
    {
        rotatingMode = rotateMode.IDLE;
        itemTransform = this.GetComponent<Transform>();
    }
    public rotateMode rotatingMode;

    void Update()
    {
        switch (rotatingMode)
        {
            case rotateMode.IDLE:
                break;
            case rotateMode.PAUSE:
                timer -= Time.deltaTime;
                if (timer <= 0 && playerIsOn == false)
                {
                    rotatingMode = rotateMode.IDLE;
                }
                break;
            case rotateMode.ROTATING:
                Rotate();
                break;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        rotatingMode = rotateMode.ROTATING;
        playerIsOn = true;
    }
    private void OnCollisionExit(Collision other)
    {
        rotatingMode = rotateMode.PAUSE;
        playerIsOn = false;
    }

    void Rotate()
    {
        itemTransform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, rotateZ * Time.deltaTime);
    }
}
