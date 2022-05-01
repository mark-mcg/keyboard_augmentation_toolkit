using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRGrabbableWithEvents : OVRGrabbable
{
    public delegate void GrabEvent();
    public event GrabEvent OnGrab, OnRelease;

    override public void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint); //pass attributes down to Super
        Debug.Log("OnGrab");
        if (OnGrab!=null) OnGrab();
    }

    override public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        Debug.Log("GrabEnd");
        if (OnRelease != null) OnRelease();
    }
}
