using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PeriodicUpdate
{
    public delegate void UpdateEvent();
    public event UpdateEvent OnUpdate;
    public float lastTime;

    [NonSerialized]
    public float interval;
    public int calls;

    public PeriodicUpdate(float inter)
    {
        this.interval = inter;
    }

    public void checkAndTrigger(float currentTime)
    {
        if (OnUpdate != null)
        {
            if (currentTime - lastTime >= interval)
            {
                lastTime = currentTime;
                OnUpdate();
                calls++;
                //Debug.Log("PeriodicUpdate checkAndTrigger called");
            }
        }
    }
}