using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using KAT.Interaction;

namespace KAT
{
    public class KUIElementDisplay : KUIElementChild
    {

    }

    public class KUISingleAreaBinaryElementDisplay : KUIElementDisplay
    {
        [BoxGroup("Binary Display Element")]
        public bool DisplayOnElementVisible = true;
        [BoxGroup("Binary Display Element")]
        public bool ShowDisplayIfVisible = false;
        [BoxGroup("Binary Display Element"), ReadOnly]
        public bool ShowingDisplay = false;

        public override void SetKUIParent(KUIElement parent)
        {
            base.SetKUIParent(parent);
            //parent.hub.Subscribe<BaseKUIEvent>(ProcessEvent);
            parent.hub.Subscribe<ActiveElementsChangedEvent>(ProcessEvent);
            parent.hub.Subscribe<LocationChangedEvent>(ProcessEvent);
            parent.hub.Subscribe<VisibilityStateEvent>(ProcessEvent);
            parent.hub.Subscribe<MappingChangedEvent>(ProcessEvent);
            parent.hub.Subscribe<LayoutChangedEvent>(ProcessEvent);
            parent.hub.Subscribe<LocationInteractionRegionChangedEvent>(ProcessEvent);
        }

        public void ProcessEvent(BaseKUIEvent @event)
        {
            Debug.Log("BinaryElementDisplay receiving event " + @event);
            if (DisplayOnElementVisible)
            {
                SetShowDisplayIfVisible(true);
            }

            if (@event is LocationInteractionRegionChangedEvent)
            {
                HideDisplay();
            }


            if (@event is LocationChangedEvent || @event is LayoutChangedEvent || @event is LocationInteractionRegionChangedEvent || @event is MappingChangedEvent)
            {
                RefreshLayout();
            }

            UpdateDisplay();
        }

        public void SetShowDisplayIfVisible(bool show)
        {
            ShowDisplayIfVisible = show;
            UpdateDisplay();
        }

        public virtual void UpdateDisplay()
        {
            if (parent != null)
            {
                if (parent.IsVisible() && ShowDisplayIfVisible)
                {
                    ShowingDisplay = true;
                    ShowDisplay();
                } else
                {
                    ShowingDisplay = false;
                    HideDisplay();
                }
            }
        }

        public virtual void ShowDisplay()
        {

        }

        public virtual void HideDisplay()
        {

        }

        public virtual void RefreshLayout()
        {

        }
    }
}