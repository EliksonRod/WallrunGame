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

            Vector3 coords = new Vector3();
            coords = hit.transform.localScale / 2f + Vector3.Scale(hit.transform.InverseTransformPoint(hit.point), hit.transform.localScale);
            if (Physics.Raycast(ray, out hit))
            {
                Transform hitTransform = hit.transform;
                if (hit.collider.gameObject.tag == "Ground")
                {
                    transform.position = hitTransform.position;
                }
            }
        }
    }
}
