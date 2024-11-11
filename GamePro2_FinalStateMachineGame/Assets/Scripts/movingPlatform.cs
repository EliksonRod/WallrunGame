using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    // Platform starting and end position values
    public Vector3 myStartPosition;
    public Vector3 myEndPosition;
    public float speed = 2f;
    public bool forward = true;

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,gameObject.transform.position.z + 1);

        //Platforms goes backward when end position is reached
        if (gameObject.transform.position.x >= myEndPosition.x)
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
    }
}
