using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMotion : MonoBehaviour
{
    public float rotateX = 100f;
    public float rotateY = 100f;
    public float rotateZ = 100f;
    public Transform itemTransform;

    // Update is called once per frame
    void Update()
    {
        //Should always multiply your movement or rotation speeds by Time.deltaTime
        itemTransform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, rotateZ * Time.deltaTime);
    }
}
