using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton{

	public bool isPressing = false;
    public bool onPressed = false;
    public bool onReleased = false;
    public bool isExtending = false;

    public float extendingDuration = 0.15f;

    private bool currentState = false;
    private bool lastState = false;

    private MyTimer extTimer = new MyTimer();

    public void Tick(bool input)
    {

        extTimer.Tick();

        currentState = input;

        isPressing = currentState;

        onPressed = false;
        onReleased = false;
        if (currentState != lastState)
        {
            if (currentState == true)
            {
                onPressed = true;
            }
            else
            {
                onReleased = true;
                StartTimer(extTimer, extendingDuration);
            }
        }

        lastState = currentState;


        if (extTimer.state == MyTimer.STATE.RUN)
        {
            isExtending = true;
        }
        else
        {
            isExtending = false;
        }
    }

    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.Go();
    }

}
