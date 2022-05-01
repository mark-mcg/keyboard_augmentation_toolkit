using KAT.KeyCodeMappings;
using NaughtyAttributes;
using UnityEngine;

namespace KAT.Interaction
{
    public class CollisionBasedKUIInteractionProvider : KUIInteractionProvider
    {
        [BoxGroup("CollisionBasedKUIInteractionProvider"), ReadOnly]
        public CollisionEventProvider siblingCollisionProvider;
        [BoxGroup("CollisionBasedKUIInteractionProvider")]
        public string CollisionTagType = "Depression";
        [BoxGroup("CollisionBasedKUIInteractionProvider")]
        public bool autoAddCollisionProvider = false;

        [ReadOnly, BoxGroup("Collision Debug")]
        public float lastCollisionTime;

        [ReadOnly, BoxGroup("Collision Debug")]
        public CollisionEvent lastCollision;

        public virtual void Awake()
        {
            CollisionEventProvider collisionProvider = this.gameObject.GetComponent<CollisionEventProvider>();

            if (collisionProvider == null && autoAddCollisionProvider)
                collisionProvider = this.gameObject.AddComponent<CollisionEventProvider>();

            if (collisionProvider != null)
            {
                collisionProvider.hub.Subscribe<CollisionEvent>(VerifyCollision);
            }
        }

        public override void SetKUIParent(KUIElement parent)
        {
            if (kuiElementParent != null)
                kuiElementParent.hub.Unsubscribe<CollisionEvent>();

            base.SetKUIParent(parent);

            if (kuiElementParent != null)
                kuiElementParent.hub.Subscribe<CollisionEvent>(VerifyCollision);
        }

        public virtual void VerifyCollision(CollisionEvent collisionEvent)
        {
            if (collisionEvent.eventProvider == siblingCollisionProvider || collisionEvent.tag.ToLower().Equals(CollisionTagType.ToLower()))
            {
                lastCollisionTime = Time.time;
                lastCollision = collisionEvent;
                ProcessCollision(collisionEvent);
            }
        }

        public virtual void ProcessCollision(CollisionEvent collisionEvent){}
    }
}