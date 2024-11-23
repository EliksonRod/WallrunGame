using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                Debug.Log("Idle");
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

        Debug.Log("Active");
    }

    void PlatformMoveBack()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(dest, transform.position, Speed);
        Vector3 movement = transform.position - old;
        /*foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }*/

        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;
            if (CurrentDest >= Destinations.Count)
                CurrentDest = 0;
        }

        Debug.Log("Returning");
    }



    /*
    // Platform starting and end position values
    public Vector3 myStartPosition;
    public Vector3 myEndPosition;
    public float speed = 2f;
    public GameObject Player;

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


    private void OnTriggerExit(Collider other)
    {
        platMode = platformMode.RETURN;
    }

    void OnTriggerStay(Collider other)
    {
        platMode = platformMode.MOVING;
    }

    void Update()
    {
        switch (platMode)
        {
            case platformMode.IDLE:
                break;
            case platformMode.MOVING:
                MoveForward();
                break;

            case platformMode.RETURN:
                GoBackward();
                break;
        }

        
    }
    void MoveForward()
    {
        //Add speed when moving forward
        if(gameObject.transform.position.x <= myEndPosition.x)
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + (Time.deltaTime * speed), gameObject.transform.position.y, gameObject.transform.position.z);
    }
    void GoBackward()
    {
        //Subtract speed when moving backward
        gameObject.transform.position = new Vector3(gameObject.transform.position.x - (Time.deltaTime * speed), gameObject.transform.position.y, gameObject.transform.position.z);
    }*/
}
