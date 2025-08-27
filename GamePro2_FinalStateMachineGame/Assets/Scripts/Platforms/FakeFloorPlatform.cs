using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class FakeFloorPlatform : MonoBehaviour
{
    //Acts like normal platform from afar but goes to position when player within set range

    //Platform Speeds
    public float Speed = 0.1f;
    public float detectionRange = 20f;

    //List of players/objects that will stay on the platform
    private List<Transform> Riders = new List<Transform>();

    [SerializeField] Transform Player_Transform;
    [SerializeField]Vector3 Original_Position;
    [SerializeField] Vector3 Target_Position;
    float distanceFromPlayer;

    void Start()
    {
        Original_Position = gameObject.transform.position;
    }
    void FixedUpdate()
    {
        KeepPlayerOnPlaform();

        distanceFromPlayer = Vector3.Distance(Player_Transform.position, gameObject.transform.position);

        //If player is further than 10 units away, go to original position, else go to target position
        if (distanceFromPlayer >= detectionRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, Original_Position, Speed);
            //transform.Translate(Original_Position * Time.deltaTime);
        }
        else
        {
            //transform.Translate(Target_Position * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, Target_Position, Speed);
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (!Riders.Contains(other.transform))
            Riders.Add(other.transform);
    }

    void OnCollisionExit(Collision other)
    {
        Riders.Remove(other.transform);
    }

    void KeepPlayerOnPlaform()
    {
        Vector3 old = transform.position;
        Vector3 movement = transform.position - old;
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }
    }
}
