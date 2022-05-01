using KAT;
using KAT.Interaction;
using NaughtyAttributes;
using PubSub;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KAT {
    /// <summary>
    /// Based on Metamorphe paper, enable basic key tilt interactions based on whether the left or right of the key is being pressed.
    /// </summary>
    public class TiltEventProvider : CollisionBasedKUIInteractionProvider
    {
        CollisionEventProvider collisionProvider;
        public bool SimulateKeyTiltBasedOnCollision = true;
        private TiltEvent.Gesture currentGesture =TiltEvent.Gesture.None;

        public override void ProcessCollision(CollisionEvent collisionEvent) {
            TiltEvent.Gesture gestEvent;

            // disable any existing depression handling temporarily
            DepressionEventProvider depressionProvider = collisionEvent.eventProviderCollider.transform.parent.GetComponentInChildren<DepressionEventProvider>();
            if (depressionProvider != null)
            {
                depressionProvider.SetDepressionState(false);
                depressionProvider.depressionEnabled = !(collisionEvent.type == CollisionType.Continue || collisionEvent.type == CollisionType.Enter);
                //Debug.LogError("Set depression enabled " + depressionProvider.depressionEnabled);
            }

            // whilst collision is occurring, receive continuous updates (so we know if we move across key surface)
            collisionEvent.descriptor.provider.SetOnStayUpdates(collisionEvent.type == CollisionType.Enter || collisionEvent.type == CollisionType.Continue);
            Debug.Log(collisionEvent.descriptor.CollisionPositions.Last().Item1.ToString("F4"));

            if (collisionEvent.type == CollisionType.Continue || collisionEvent.type == CollisionType.Enter)
            {
                if (collisionEvent.descriptor.CollisionPositions.Last().Item1.x < 0)
                {
                    gestEvent = TiltEvent.Gesture.Right;
                }
                else
                {
                    gestEvent = TiltEvent.Gesture.Left;
                }

            } else {
                gestEvent = TiltEvent.Gesture.None;            
            }

            Transform key = GetKeyTransform();
            if (key != null && SimulateKeyTiltBasedOnCollision)
            {
                switch (gestEvent)
                {
                    case TiltEvent.Gesture.Left:
                        key.localEulerAngles = new Vector3(0, 20, 0);
                        break;
                    case TiltEvent.Gesture.Right:
                        key.localEulerAngles = new Vector3(0, -20, 0);
                        break;
                    case TiltEvent.Gesture.None:
                        key.localEulerAngles = Vector3.zero;
                        break;
                }
            }

            if (currentGesture != gestEvent)
            {
                Debug.Log("Simulated GestAKey event " + gestEvent);
                currentGesture = gestEvent;
                hub.Publish(new TiltEvent(gestEvent));
            }
        }

        private Transform GetKeyTransform()
        {
            return GetInteractionManager()?.GetLocation()?.LayoutLocation?.transform;
        }
    }
}
