using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using System.Linq;

public class Elevator : MonoBehaviour
{
    public List<Vector3> Destinations;
    List<Transform> Riders = new List<Transform>();
    int CurrentDest;
    bool playerIsOn = false;

    public float Speed = 0.1f;

    public float DestTimer = 4;
    float destTimer;

    public enum platformMode
    {
        IDLE,
        MOVING,
        ATSTOP,
        RETURN
    }
    private void Start()
    {
        destTimer = DestTimer;
        platMode = platformMode.IDLE;
    }
    public platformMode platMode;

    void FixedUpdate()
    {
        //Different modes the platform will switch through
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

        playerIsOn = true;
    }

    private void OnCollisionExit(Collision other)
    {
        Riders.Remove(other.transform);
        playerIsOn = false;
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
        }
        if (Vector3.Distance(transform.position, lastStop) < 0.01f)
        {
            destTimer = DestTimer;
            platMode = platformMode.IDLE;
        }
    }
}
