using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton{

	public bool isPressing = false;
    public bool onPressed = false;
    public bool onReleased = false;

    private bool currentState = false;
    private bool lastState = false;

    public void Tick(bool input)
    {
        currentState = input;

        isPressing = currentState;

        onPressed = false;
        onReleased = false;
        if(currentState != lastState)
        {
            if(currentState == true)
            {
                onPressed = true;
            }
            else
            {
                onReleased = true;
            }
        }

        lastState = currentState;

    }

}
