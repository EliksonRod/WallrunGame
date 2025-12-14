using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static StateRotation;
using System.Linq;

public class StateLoopingPlatform : MonoBehaviour
{
    //Will NOT MOVE at start, activated by player collision, does NOT wait ATSTOP
    //Will MOVE between destinations (back and forth) forever once activated

    public List<Vector3> Destinations;
    private int CurrentDest;
    public float Speed = 0.1f;
    private List<Transform> Riders = new List<Transform>();
    public enum platformMode
    {
        IDLE,
        MOVING,
    }
    private void Start()
    {
        platMode = platformMode.IDLE;
    }
    public platformMode platMode;

    void FixedUpdate()
    {
        switch (platMode)
        {
            case platformMode.IDLE:
                break;
            case platformMode.MOVING:
                PlatformActive();
                break;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!Riders.Contains(other.transform))
            Riders.Add(other.transform);

        platMode = platformMode.MOVING;
    }

    private void OnCollisionExit(Collision other)
    {
        Riders.Remove(other.transform);
    }

    void PlatformActive()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, Speed);
        Vector3 movement = transform.position - old;
        Vector3 lastStop = Destinations.Last();
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }

        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;

            if (CurrentDest >= Destinations.Count)
            {
                CurrentDest = 0;
            }
        }
    }
}
