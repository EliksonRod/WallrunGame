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
    [SerializeField]Vector3 Target_Position;
    float distanceFromPlayer;
    float distanceFromOriginalLocation;
    bool player_In_Range = false;

    void Start()
    {
        Original_Position = gameObject.transform.position;
    }
    void FixedUpdate()
    {
        KeepPlayerOnPlaform();

        distanceFromPlayer = Vector3.Distance(Player_Transform.position, gameObject.transform.position);
        distanceFromOriginalLocation = Vector3.Distance(Player_Transform.position, Original_Position);

        //If player is far enough from platform or platform's original location, platform returns to original location
        if (distanceFromPlayer >= detectionRange && distanceFromOriginalLocation >= detectionRange)
        {
            player_In_Range = false;
        }
        else if (distanceFromPlayer <= detectionRange || distanceFromOriginalLocation <= detectionRange)
        {
            player_In_Range = true;
        }


        if (player_In_Range == true)
        {
            Debug.Log("Player in range");
            transform.position = Vector3.MoveTowards(transform.position, Target_Position, Speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Original_Position, Speed);
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
