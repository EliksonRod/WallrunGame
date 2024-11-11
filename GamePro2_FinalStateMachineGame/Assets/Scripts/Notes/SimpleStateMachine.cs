using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStateMachine : MonoBehaviour
{
    //heres my enumerable, this will track the color of the object's material 
     public enum stateMode
    {
        RED,
        GREEN, 
        BLUE
    }
    //an actual instance of the enumerable stateMode we have just defined
    public stateMode myMode;
    Renderer myRend;
    Material myMat;

    // Start is called before the first frame update
    void Start()
    {
        myRend= GetComponent<Renderer>();
        myMat = myRend.material;
    }

    // Update is called once per frame
    void Update()
    {
        //to determine what code ti run based off the current state of our myMode variable 
        //we're going to use a switch statement
        switch (myMode)//declare the enum this is referencing
        {
            case stateMode.RED://fir each potential state or mode, declare a "case" and then write an relevant code for thay mode
                myMat.color = Color.red;
                break;//at the end of each case, put a break

            case stateMode.GREEN:
                myMat.color = Color.green;
                break;

            case stateMode.BLUE:
                myMat.color = Color.blue;
                break;
        }
        //regular update code goes here, probably something that depends 
    }
}
