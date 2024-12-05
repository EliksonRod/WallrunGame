using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRotation : MonoBehaviour
{
    public float rotateX = 100f;
    public float rotateY = 100f;
    public float rotateZ = 100f;
    private Transform itemTransform;
    public enum rotateMode
    {
        IDLE,
        MOVING
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
            case rotateMode.MOVING:
                Rotate();
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        rotatingMode = rotateMode.IDLE;
    }

    void OnTriggerStay(Collider other)
    {
        rotatingMode = rotateMode.MOVING;
    }

    void Rotate()
    {
        itemTransform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, rotateZ * Time.deltaTime);
    }
}
