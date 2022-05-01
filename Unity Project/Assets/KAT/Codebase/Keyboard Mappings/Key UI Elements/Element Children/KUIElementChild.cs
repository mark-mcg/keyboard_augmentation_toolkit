using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT
{
    public class KUIElementChild : KUIParentChildEventProvider, KUIEvents.KUIElementDepedent
    {
        [BoxGroup("KUIElementChild"), ReadOnlyAttribute]
        public bool ChildActive = false;

        [HideInInspector]
        public KUIElement parent;

        public virtual void SetKUIParent(KUIElement parent)
        {
            this.parent = parent;

            if (parent != null)
            {
                parent.hub.Subscribe<BaseKUIEvent>(@event =>
               {
                   //Debug.LogFormat("KUIElementChild received event {0} ", @event);
                   SetActive(parent.IsVisible());
               });
            }
        }
        public virtual void SetActive(bool active)
        {
            // Debug.LogFormat("SetActive for KUIElementChild {0} state {1}", this, active);
            this.ChildActive = active;
        }
    }
}