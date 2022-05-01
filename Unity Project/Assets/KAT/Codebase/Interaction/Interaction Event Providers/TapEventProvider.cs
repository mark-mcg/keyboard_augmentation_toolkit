using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT.Interaction
{
    public enum TapType { None, Single, Double, Triple }
    public class TapEvent : BaseKUIEvent
    {
        public TapType TapType;
        public GameObject originator;

        public TapEvent(GameObject originator)
        {
            this.originator = originator;
        }
    }

    public class TapEventProvider : CollisionBasedKUIInteractionProvider
    {
        [BoxGroup("Tap Configuration")]
        public float TapMinDuration = 0.05f;
        [BoxGroup("Tap Configuration")]
        public float TapMaxDuration = 0.4f;
        [BoxGroup("Tap Configuration")]
        public float InterTapMaxDuration = 1.0f;
        [BoxGroup("Tap Configuration")]
        public float TapExpiryTime = 3.0f;
        [BoxGroup("Tap Configuration")]
        public float AllowableDistanceFromCenter = 1.2f;

        [ReadOnly, BoxGroup("Tap Debug")] 
        public Collider lastCollider;
        [ReadOnly, BoxGroup("Tap Debug")]
        public List<float> tapEvents = new List<float>();
        [ReadOnly, BoxGroup("Tap Debug")]
        public bool changingPosition = false;
        [ReadOnly, BoxGroup("Tap Debug")]
        Vector3 prevPosition;
        [ReadOnly, BoxGroup("Tap Debug")]
        public TapType lastTapType = TapType.None;
        [ReadOnly, BoxGroup("Tap Debug")]
        Vector3 enterPosition;
        [ReadOnly, BoxGroup("Tap Debug")]
        float lastCollisionStartTime;
        public override void ProcessCollision(CollisionEvent @event)
        {
            base.ProcessCollision(@event);
            @event.eventProvider.SetOnStayUpdates(true);
            BoxCollider collider = (BoxCollider)@event.eventProvider.GetCollider();

            if (!changingPosition)
            {
                if (@event.type == CollisionType.Enter)
                {
                    if (lastCollider == null || @event.collidingCollider == lastCollider)
                    {
                        lastCollider = @event.collidingCollider;
                        lastCollisionStartTime = Time.time;
                        enterPosition = lastCollider.transform.InverseTransformPoint(@event.collidingCollider.transform.position);
                    }
                    else
                    {
                        // we've had multiple collisions from different colliders - assume we can't differentiate a tap (we don't support multitouch currently)
                        lastCollider = null;
                    }
                }

                if (@event.type == CollisionType.Exit)
                {
                    if (lastCollider == @event.collidingCollider)
                    {
                        float duration = Time.time - lastCollisionStartTime;
                        // was the exit point roughly centered in our tap collider on x/y (ignoring z)
                        // if so, this constitutes a tap - ignoring swipe events where exit points would be at the extremes of x/y
                        float distanceFromCenter = Vector2.Distance(collider.center, collider.transform.InverseTransformPoint(@event.collidingCollider.transform.position));
                        float distanceToBounds = collider.bounds.extents.magnitude;
                        Vector3 fromTo = collider.transform.InverseTransformPoint(@event.collidingCollider.transform.position) - enterPosition;
                        float xyDistance = new Vector3(fromTo.x, fromTo.y).magnitude;

                        //Debug.LogErrorFormat("Duration: {0}, distanceFromCenter: {1}, distanceToBounds: {2}, xyDistance: {3}, allowableDistance * distanceToBounds {4}",
                        //    duration, distanceFromCenter, distanceToBounds, xyDistance, (distanceToBounds * AllowableDistanceFromCenter));

                        if (duration >= TapMinDuration && duration <= TapMaxDuration && ((distanceFromCenter < distanceToBounds * AllowableDistanceFromCenter) && (xyDistance < distanceToBounds * AllowableDistanceFromCenter)))
                        {
                            Debug.LogError("Tap detected");
                            tapEvents.Add(lastCollisionStartTime);
                            TapEvent tapEvent = new TapEvent(@event.collidingCollider.gameObject);
                            tapEvent.TapType = TapType.Single;

                            if (tapEvents.Count > 3)
                                tapEvents.RemoveRange(0, tapEvents.Count - 3);

                            // check for double tap
                            if (tapEvents.Count >= 2)
                            {
                                if (Mathf.Abs(tapEvents[tapEvents.Count - 2] - tapEvents.Last()) <= InterTapMaxDuration)
                                    tapEvent.TapType = TapType.Double;
                            }

                            // check for triple tap
                            if (tapEvents.Count == 3 && tapEvent.TapType == TapType.Double)
                            {
                                if (Mathf.Abs(tapEvents[tapEvents.Count - 3] - tapEvents[tapEvents.Count - 2]) <= InterTapMaxDuration)
                                    tapEvent.TapType = TapType.Triple;
                            }

                            lastTapType = tapEvent.TapType;
                            hub.Publish(tapEvent);
                        }
                        lastCollider = null;
                    }
                }
            }
        }

        public void Update()
        {
            // invalidate any taps if the position changes (i.e. key depressed, keyboard moved)
            changingPosition = prevPosition != transform.position;

            if (changingPosition)
            {
                lastCollisionStartTime = 0;
                lastCollider = null;
            }
            prevPosition = transform.position;

            if (lastTapType != TapType.None && (Time.time-tapEvents.Last()) > TapExpiryTime)
            {
                lastTapType = TapType.None;
                TapEvent @event = new TapEvent(this.gameObject);
                @event.TapType = lastTapType;
                hub.Publish(@event);
            }
        }
    }
}
