//	The MIT License (MIT)
//	
//	Copyright (c) 2015 neervfx
//		
//	Permission is hereby granted, free of charge, to any person obtaining a copy
//	of this software and associated documentation files (the "Software"), to deal
//	in the Software without restriction, including without limitation the rights
//	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//	copies of the Software, and to permit persons to whom the Software is
//	furnished to do so, subject to the following conditions:
//		
//	The above copyright notice and this permission notice shall be included in all
//	copies or substantial portions of the Software.
//		
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//	SOFTWARE.

// Adapted from  https://github.com/neervfx/swipe/blob/master/SwipeManager.cs
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using Boo.Lang;

namespace KAT.Interaction
{
    public class SwipeEvent : BaseKUIEvent
    {
        public GameObject originator;
        public SwipeEventProvider.Swipes direction;

        public SwipeEvent(GameObject originator, SwipeEventProvider.Swipes direction)
        {
            this.originator = originator;
            this.direction = direction;
        }
    }

    /// <summary>
    /// Based on GestAKey paper.
    /// </summary>
    public class SwipeEventProvider : CollisionBasedKUIInteractionProvider
    {
        public bool LogThis = false;
        public enum Swipes { None, Up, Down, Left, TopLeft, BottomLeft, Right, TopRight, BottomRight };
        public List<Swipes> HorizontalSwipes = new List<Swipes>() { Swipes.Left, Swipes.Right, Swipes.BottomLeft, Swipes.BottomRight, Swipes.TopLeft, Swipes.TopRight };
        public List<Swipes> VerticalSwipes = new List<Swipes>() { Swipes.Up, Swipes.Down, Swipes.BottomLeft, Swipes.BottomRight, Swipes.TopLeft, Swipes.TopRight };


        [BoxGroup("SwipeEventProvider")]
        public float minSwipeLengthAsRatio = 0.6f;
        [BoxGroup("SwipeEventProvider")]
        public float minSwipeVelocity = 0.01f;
        [BoxGroup("SwipeEventProvider"), ReadOnly]
        Vector2 currentSwipeDistance;
        [BoxGroup("SwipeEventProvider"), ReadOnly]
        public Swipes direction;

        [BoxGroup("SwipeEventProvider"), ReadOnly]
        public float DebugSwipeLength, DebugSwipeVelocity;

        private Vector2 swipeMaxRange;

        public override void ProcessCollision(CollisionEvent collisionEvent)
        {
            base.ProcessCollision(collisionEvent);
            collisionEvent.eventProvider.SetOnStayUpdates(true);
            BoxCollider collider = (BoxCollider) collisionEvent.eventProvider.GetCollider();

            //if (LogThis)
            //Debug.Log("Swipe provider got collision event, last position was " + castEvent.descriptor.CollisionPositions.Last().Item1.ToString("F4") + " count == " + castEvent.descriptor.CollisionPositions.Count());
            Swipes lastDirection = direction;
            SwipeDetection(collisionEvent, collider);
            if (LogThis)
            {
                Debug.Log("Swipe event direction: " + direction + " last direction " + lastDirection + " equals " + (direction != lastDirection));
            }

            if (direction != lastDirection && direction != Swipes.None)
            {
                if (LogThis)
                {
                    Debug.LogError("Swipe event direction: " + direction + " last direction " + lastDirection + " equals " + (direction != lastDirection));
                }
                Debug.LogError("Swipe event direction: " + direction + " last direction " + lastDirection + " equals " + (direction != lastDirection));

                hub.Publish(new SwipeEvent(collisionEvent.collidingCollider.gameObject, direction));
            }
        }


        public void SwipeDetection(CollisionEvent @event, BoxCollider collider)
        {
            if (@event.type == CollisionType.Continue || @event.type == CollisionType.Exit)
            {
                swipeMaxRange = new Vector2(Mathf.Abs(collider.size.x), Mathf.Abs(collider.size.y));

                currentSwipeDistance = new Vector2(
                    @event.descriptor.CollisionPositions.Last().Item1.x - @event.descriptor.CollisionPositions.First().Item1.x, 
                    @event.descriptor.CollisionPositions.Last().Item1.y - @event.descriptor.CollisionPositions.First().Item1.y);

                // first, check that the horizontal and/or vertical components constitute a swipe
                float timeElapsed = @event.descriptor.CollisionPositions.Last().Item3 - @event.descriptor.CollisionPositions.First().Item3;
                float velocity = currentSwipeDistance.magnitude / timeElapsed;

                bool swipeVelocityCheck = velocity > minSwipeVelocity;
                bool horizontalSwipeCheck = Mathf.Abs(currentSwipeDistance.x) > (minSwipeLengthAsRatio * swipeMaxRange.x);
                bool verticalSwipeCheck = Mathf.Abs(currentSwipeDistance.y) > (minSwipeLengthAsRatio * swipeMaxRange.y);

                if (LogThis)
                    Debug.Log("Vertical distance " + currentSwipeDistance.y + " min distance " + (minSwipeLengthAsRatio * swipeMaxRange.y));


                DebugSwipeLength = currentSwipeDistance.magnitude;
                DebugSwipeVelocity = velocity;
                currentSwipeDistance.x *= -1; // swap left/right

                if (!(swipeVelocityCheck && (horizontalSwipeCheck || verticalSwipeCheck)))
                {
                    //if (LogThis)
                    //    Debug.Log("Failed checks timeElapsed " + timeElapsed + " velocity " + velocity + "swipeVelocityCheck" + swipeVelocityCheck + " horizontalSwipeCheck" + horizontalSwipeCheck + " verticalSwipeCheck" + verticalSwipeCheck);
                    direction = Swipes.None;
                    return;
                }
                else
                {
                    // we have a swipe, now to determine direction
                    float angle = (Mathf.Atan2(currentSwipeDistance.y, currentSwipeDistance.x) / (Mathf.PI));
                    if (LogThis)
                        Debug.Log(angle + " from " + currentSwipeDistance);
                    // Swipe up
                    if (angle > 0.375f && angle < 0.625f)
                    {
                        direction = Swipes.Up;
                        // Swipe down
                    }
                    else if (angle < -0.375f && angle > -0.625f)
                    {
                        direction = Swipes.Down;
                        // Swipe left
                    }
                    else if (angle < -0.875f || angle > 0.875f)
                    {
                        direction = Swipes.Left;
                        // Swipe right
                    }
                    else if (angle > -0.125f && angle < 0.125f)
                    {
                        direction = Swipes.Right;
                    }
                    else if (angle > 0.125f && angle < 0.375f)
                    {
                        direction = Swipes.TopRight;
                    }
                    else if (angle > 0.625f && angle < 0.875f)
                    {
                        direction = Swipes.TopLeft;
                    }
                    else if (angle < -0.125f && angle > -0.375f)
                    {
                        direction = Swipes.BottomRight;
                    }
                    else if (angle < -0.625f && angle > -0.875f)
                    {
                        direction = Swipes.BottomLeft;
                    }
                }
            }
        }
    }
}