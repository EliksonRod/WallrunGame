using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class movingPlatform : MonoBehaviour
{
    // Platform starting and end position values
    public Vector3 myStartPosition;
    public Vector3 myEndPosition;
    public float speed = 2f;
    public bool forward = true;

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

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }*/

    private void OnTriggerExit(Collider other)
    {
        /*if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }*/

        platMode = platformMode.RETURN;
    }

    void OnTriggerStay(Collider other)
    {
        platMode = platformMode.MOVING;
    }

    void Update()
    {
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,gameObject.transform.position.z + 1);

        //Platforms goes backward when end position is reached
        /*if (gameObject.transform.position.x >= myEndPosition.x)
        {
            forward = false;
        }
        //Platform switches forward when reaching start position 
        else if (gameObject.transform.position.x <= myStartPosition.x)
        {
            forward = true;
        }

        //Add speed when moving forward
        if (forward == true)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + (Time.deltaTime * speed), gameObject.transform.position.y, gameObject.transform.position.z);
        }
        //Subtract speed when going backward
        else if (forward == false)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - (Time.deltaTime * speed), gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if(gameObject.transform.position.x == myStartPosition.x)
        {
            platMode = platformMode.IDLE;
        }*/
        

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
    }
}
