using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KAT.Interaction;

namespace KAT
{
    public class KUIBinaryElementAction : KUIElementChild
    {
        public override void SetKUIParent(KUIElement parent)
        {
            base.SetKUIParent(parent);
            parent.hub.Subscribe<KUIInteractionEvent>(interactionEvent =>
            {
                if (interactionEvent.id == KUIInteractionEvent.Interaction.Activate)
                    PerformAction();

                if (interactionEvent.id == KUIInteractionEvent.Interaction.Inactive)
                    StopAction();
            });
        }

        public virtual void PerformAction()
        {
            Debug.LogFormat("Performing action for {0}", this);
        }

        public virtual void StopAction()
        {

        }
    }
}
