using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class teleport : MonoBehaviour
{
    void Update()
    {
        MyInput();
    }

    void MyInput()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var localHit = transform.InverseTransformPoint(hit.point);
                Transform hitTransform = hit.transform;
                if (hit.collider.gameObject.tag == "Ground")
                {
                    //transform.position = hitTransform.position;
                    transform.position = localHit;
                }
            }
        }
    }
}
