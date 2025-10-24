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
    private List<Transform> Riders = new List<Transform>();
    public enum rotateMode
    {
        IDLE,
        ROTATING,
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
            case rotateMode.ROTATING:
                Rotate();
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Riders.Contains(other.transform))
            Riders.Add(other.transform);

            rotatingMode = rotateMode.ROTATING;
    }

    private void OnCollisionExit(Collision other)
    {
        Riders.Remove(other.transform);
    }

    void Rotate()
    {
        itemTransform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, rotateZ * Time.deltaTime);

        Vector3 old = transform.position;
        Vector3 movement = transform.position - old;
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }
    }
}
