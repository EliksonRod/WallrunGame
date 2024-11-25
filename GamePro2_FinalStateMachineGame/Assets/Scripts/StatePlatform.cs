using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static StateRotation;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class StatePlatform : MonoBehaviour
{
    public List<Vector3> Destinations;
    private int CurrentDest;
    public float Speed = 0.1f;
    private List<Transform> Riders = new List<Transform>();

    public enum platformMode
    {
        IDLE,
        MOVING,
        RETURN
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
                //Debug.Log("Idle");
                break;
            case platformMode.MOVING:
                PlatformActive();
                break;
            case platformMode.RETURN:
                PlatformMoveBack();
                break;
        }

        /*if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, Speed);
        Vector3 movement = transform.position - old;
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }

        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;
            if (CurrentDest >= Destinations.Count)
                CurrentDest = 0;
        }*/
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

        platMode = platformMode.RETURN;
    }

    void PlatformActive()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, Speed);
        Vector3 movement = transform.position - old;
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }

        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;
            if (CurrentDest >= Destinations.Count)
                CurrentDest = 0;
        }
        //Debug.Log("Active");
    }

    void PlatformMoveBack()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(dest, transform.position, Speed);
        Vector3 movement = transform.position - old;


        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }

        if (Vector3.Distance(dest, transform.position) < 0.01f)
        {
            CurrentDest++;
            if (CurrentDest >= Destinations.Count)
                CurrentDest = 0;
        }

        //Debug.Log("Returning");
    }
}
