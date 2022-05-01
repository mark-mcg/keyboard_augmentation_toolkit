using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT.Interaction
{
    public interface IKUIInteractionProvider
    {
        void SetLocationInteractionRegion(KUILocationInteractionRegions region);
    }

    public class KUIInteractionProvider : KUIParentChildEventProvider, IKUIInteractionProvider
    {
        public Hub hub = new Hub();

        public virtual void Start()
        {
           FindObjectOfType<KUIManager>().NonKeyEvents.Subscribe<LayoutChangedEvent>(@event =>
           {
                LayoutChanged();
           });

            // relay events to interactionManager, if present
            hub.Subscribe<BaseKUIEvent>(@event =>
            {
                interactionManager?.GetLocation()?.hub.Publish(@event);
            });
        }

        private KUILocationInteractionRegions interactionManager;

        public void SetLocationInteractionRegion(KUILocationInteractionRegions region)
        {
            interactionManager = region;
            LayoutChanged();
        }

        public KUILocationInteractionRegions GetInteractionManager()
        {
            return interactionManager;
        }

        public virtual void LayoutChanged() { }
    }
}