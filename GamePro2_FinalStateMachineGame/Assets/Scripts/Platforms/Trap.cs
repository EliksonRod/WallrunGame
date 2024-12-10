using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static StateRotation;
using System.Linq;

public class Trap : MonoBehaviour
{
    //Will NOT MOVE at start, activated by player collision, does NOT wait ATSTOP
    //It will loop between destinations (back and forth) as long as player is on it and will pause in its tracks when jumped off/exited

    public List<Vector3> Destinations;
    private int CurrentDest;
    public float Speed = 0.1f;
    private List<Transform> Riders = new List<Transform>();
    public float timer = 4;
    public float DestTimer = 4;
    public enum platformMode
    {
        IDLE,
        TRAPACTIVE,
        RESET,
        TRAPUSED
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
            case platformMode.TRAPACTIVE:
                TrapActive();
                break;
            case platformMode.RESET:
                ResetTrap();
                break;
            case platformMode.TRAPUSED:
                DestTimer -= Time.deltaTime;
                if (DestTimer <= 0)
                {
                    platMode = platformMode.RESET;

                    CurrentDest--;
                    if (CurrentDest < 0)
                        CurrentDest = Destinations.Count - 1;
                }
                break;
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        platMode = platformMode.TRAPACTIVE;
    }

    void TrapActive()
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
            if (Vector3.Distance(transform.position, lastStop) < 0.01f)
            {
                DestTimer = 4;
                platMode = platformMode.TRAPUSED;
            }
        }
    }

    void ResetTrap()
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
            CurrentDest--;
            if (CurrentDest < 0)
            {
                CurrentDest = 0;
                platMode = platformMode.IDLE;
            }
        }
    }
}
