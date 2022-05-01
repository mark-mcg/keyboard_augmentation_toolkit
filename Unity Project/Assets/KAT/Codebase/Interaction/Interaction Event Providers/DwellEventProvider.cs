using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT.Interaction
{
    public class DwellEvent : BaseKUIEvent
    {
        public GameObject originator;
        public bool Dwelling = false;

        public DwellEvent(GameObject originator, bool happening)
        {
            this.originator = originator;
            Dwelling = happening; 
        }
    }

    public class DwellEventProvider : CollisionBasedKUIInteractionProvider
    {
        [ReadOnly, BoxGroup("Dwell configuration")]
        public float DwellMinDuration = 1.0f;
        [ReadOnly, BoxGroup("Dwell debug")]
        Coroutine dwellCoroutine;
        [ReadOnly, BoxGroup("Dwell debug")]
        Collider lastCollider;

        public override void ProcessCollision(CollisionEvent @event)
        {
            base.ProcessCollision(@event);
            @event.eventProvider.SetOnStayUpdates(true);
            BoxCollider collider = (BoxCollider)@event.eventProvider.GetCollider();

            if (@event.type == CollisionType.Enter)
            {
                if (lastCollider == null || @event.collidingCollider == lastCollider)
                {
                    StartDwell(@event);
                }
                else
                {
                    // we've had multiple collisions from different colliders - assume we can't differentiate a tap (we don't support multitouch currently)
                    StopDwell();
                }
            }

            if (@event.type == CollisionType.Exit)
            {
                if (lastCollider == @event.collidingCollider)
                {
                    StopDwell();
                }
            }
        }

        private void StartDwell(CollisionEvent @event)
        {
            lastCollider = @event.collidingCollider;
            dwellCoroutine = StartCoroutine(DwellCoroutine());
        }

        private bool dwelling = false;
        public IEnumerator DwellCoroutine()
        {
            yield return new WaitForSeconds(DwellMinDuration);
            dwelling = true;
            hub.Publish(new DwellEvent(lastCollider.gameObject, true));
        }

        private void StopDwell()
        {
            StopCoroutine(dwellCoroutine);
            if (dwelling)
                hub.Publish(new DwellEvent(lastCollider.gameObject, false));

            lastCollider = null;
        }
    }
}
