using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using OVRTouchSample;

public class OVRHandTrackingGrabber : OVRGrabber
{
    public float pinchThreshold = 0.7f;
    public float currentPinchStrength;
    private OVRHand hand;
    private OVRFingertipTracking fingertipTracker;
    private SphereCollider collider;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hand = GetComponentInChildren<OVRHand>();
        fingertipTracker = GetComponent<OVRFingertipTracking>();
        collider = GetComponent<SphereCollider>();
    }

    private bool setup = false;
    public override void Update()
    {
        base.Update();
        if (fingertipTracker.fingertipsFound)
        {
            collider.center = fingertipTracker.fingertipGameObjects["Hand_IndexTip"].transform.localPosition;
            CheckIndexPinch();
        }
    }

    public void CheckIndexPinch()
    {
        currentPinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = currentPinchStrength > pinchThreshold && hand.GetFingerConfidence(OVRHand.HandFinger.Index) == OVRHand.TrackingConfidence.High;

        //if (m_grabCandidates.Count > 0)
        //    Debug.Log("Got some grab candidates: " + m_grabCandidates.Count);
        if (!m_grabbedObj && isPinching && m_grabCandidates.Count > 0)
        {
            GrabBegin();
        } else if (m_grabbedObj && !isPinching)
        {
            GrabEnd();
        }
    }
}
