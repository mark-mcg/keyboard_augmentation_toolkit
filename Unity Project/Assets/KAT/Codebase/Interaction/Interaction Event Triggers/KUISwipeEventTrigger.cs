using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT.Interaction
{
    [Serializable]
    public class UnitySwipeEvent : UnityEvent<SwipeEventProvider.Swipes> { }

    public class KUISwipeEventTrigger : KUIGestureSurfaceEventTrigger<SwipeEvent>
    {
        [BoxGroup("Swipe Events")]
        public bool TriggerOnSwipe = true;
        [BoxGroup("Swipe Events")]
        public SwipeEventProvider.Swipes TriggerOn;
        [BoxGroup("Swipe Events (Unity)")]
        public UnityEvent OnLeftSwipe, OnRightSwipe, OnUpSwipe, OnDownSwipe, OnTopLeftSwipe, OnBottomLeftSwipe, OnTopRightSwipe, OnBottomRightSwipe;
        [BoxGroup("Swipe Events (Unity)")]
        public UnitySwipeEvent OnSwipe;

        public override bool Matches(KUIElementBaseTrigger trigger)
        {
            if (trigger == null)
                return false;
            if (trigger is KUISwipeEventTrigger castEvent)
                return this.TriggerOn == castEvent.TriggerOn;
            return false;
        }

        public override void ProcessEvent(SwipeEvent castEvent)
        {
            Debug.Log("Received swipe event on trigger!");
            SetTriggerState(castEvent.direction == TriggerOn && TriggerOnSwipe);

            switch (castEvent.direction)
            {
                case SwipeEventProvider.Swipes.Up:
                    OnUpSwipe?.Invoke();
                    break;
                case SwipeEventProvider.Swipes.Down:
                    OnDownSwipe?.Invoke();
                    break;
                case SwipeEventProvider.Swipes.Left:
                    OnLeftSwipe?.Invoke();
                    break;
                case SwipeEventProvider.Swipes.TopLeft:
                    OnTopLeftSwipe?.Invoke();
                    break;
                case SwipeEventProvider.Swipes.BottomLeft:
                    OnBottomLeftSwipe?.Invoke();
                    break;
                case SwipeEventProvider.Swipes.Right:
                    OnRightSwipe?.Invoke();
                    break;
                case SwipeEventProvider.Swipes.TopRight:
                    OnTopRightSwipe?.Invoke();
                    break;
                case SwipeEventProvider.Swipes.BottomRight:
                    OnBottomRightSwipe?.Invoke();
                    break;
            }

            OnSwipe?.Invoke(castEvent.direction);
        }
    }
}