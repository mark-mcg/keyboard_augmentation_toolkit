using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT.Interaction
{
    [Serializable]
    public class UnityTiltEvent : UnityEvent<TiltEvent.Gesture> { }

    public class TiltEventTrigger : KUIElementBaseTrigger<TiltEvent>
    {
        [BoxGroup("SimulatedGestAKeyEvents (Unity)")]
        public UnityEvent OnLeft, OnRight, OnNone;

        [BoxGroup("Swipe Events (Unity)")]
        public UnityTiltEvent OnGestAKeyEvent;

        public override bool Matches(KUIElementBaseTrigger trigger)
        {
            return false;
        }

        public override void ProcessEvent(TiltEvent castEvent)
        {
            Debug.Log("Received swipe event on trigger!");

            switch (castEvent.gesture)
            {
                case TiltEvent.Gesture.Left:
                    OnLeft?.Invoke();
                    break;
                case TiltEvent.Gesture.Right:
                    OnRight?.Invoke();
                    break;
                case TiltEvent.Gesture.None:
                    OnNone?.Invoke();
                    break;
            }

            OnGestAKeyEvent?.Invoke(castEvent.gesture);
        }
    }
}