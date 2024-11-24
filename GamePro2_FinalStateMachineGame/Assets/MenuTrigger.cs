using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    public GameObject menuPopUp;

    private void OnTriggerEnter(Collider other)
    {
        menuPopUp.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        Destroy(menuPopUp); 
    }
}
