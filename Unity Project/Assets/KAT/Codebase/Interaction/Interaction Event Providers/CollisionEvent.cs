using System;
using UnityEngine;

namespace KAT.Interaction
{
    [Serializable]
    public class CollisionEvent : BaseKUIEvent
    {
        public CollisionEventProvider eventProvider;
        public Collider collidingCollider, eventProviderCollider;
        public CollisionDescriptor descriptor;
        public CollisionType type;
        public string tag;

        public CollisionEvent(CollisionEventProvider provider, Collider collider, Collider eventProvider, CollisionDescriptor descriptor, CollisionType type, string tag)
        {
            this.eventProvider = provider;
            this.collidingCollider = collider;
            this.descriptor = descriptor;
            this.type = type;
            this.eventProviderCollider = eventProvider;
            this.tag = tag;
        }

        public override string ToString()
        {
            return String.Format("CollisionEvent type {0} tag {1}", type, tag); 
        }
    }
}