using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton{

	public bool isPressing = false;
    public bool onPressed = false;
    public bool onReleased = false;
    public bool isExtending = false;
    public bool isDelaying = false;

    //双击间隔时间
    public float extendingDuration = 0.3f;

    //长按 需求时间
    public float delayingDuration = 0.4f;

    private bool currentState = false;
    private bool lastState = false;

    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();

    public void Tick(bool input)
    {

        extTimer.Tick();
        delayTimer.Tick();

        currentState = input;

        isPressing = currentState;

        onPressed = false;
        onReleased = false;
        isExtending = false;
        isDelaying = false;

        if (currentState != lastState)
        {
            if (currentState == true)
            {
                onPressed = true;
                StartTimer(delayTimer, delayingDuration); //按下 长按计时
            }
            else
            {
                onReleased = true;
                StartTimer(extTimer, extendingDuration); //释放后 双击接受计时
            }
        }

        lastState = currentState;


        if (extTimer.state == MyTimer.STATE.RUN)
        {
            isExtending = true;
        }

        if(delayTimer.state == MyTimer.STATE.RUN)
        {
            isDelaying = true;
        }
    }

    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.Go();
    }

    public void ResetTimer()
    {
        extTimer.Reset();
        delayTimer.Reset();
    }

}
