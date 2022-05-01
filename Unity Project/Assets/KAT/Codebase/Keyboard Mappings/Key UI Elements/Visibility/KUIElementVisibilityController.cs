using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAT
{
    public abstract class KUIElementVisibilityController : KUIParentChildEventProvider, KUIEvents.KUIElementDepedent
    {
        public abstract KUIElement.VisibilityState CheckVisibilityState();

        private KUIElement parent;
        public void SetKUIParent(KUIElement parent)
        {
            this.parent = parent;
            this.parent.hub.Subscribe<BaseKUIEvent>(@event =>
            {
                //Debug.Log("Visibility controller event " + @event);
                // update our visibility state, and force the update if the @event indicates a change in the active elements
                if (!(@event is VisibilityStateEvent))
                    parent.SetVisibilityState(CheckVisibilityState(), @event is ActiveElementsChangedEvent);
            });
        }
    }
}
