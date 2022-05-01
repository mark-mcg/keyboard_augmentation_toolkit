using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT.Interaction
{
    public class KUIInteractionEventTrigger : KUIElementBaseTrigger<KUIInteractionEvent>
    {
        public KUIInteractionEvent.Interaction Start, Stop;

        public override void ProcessEvent(KUIInteractionEvent castEvent)
        {
            base.ProcessEvent(castEvent);
            if (castEvent.id == Start)
                SetTriggerState(true);

            if (castEvent.id == Stop)
                SetTriggerState(false);
        }

        public override bool Matches(KUIElementBaseTrigger trigger)
        {
            if (trigger == null)
                return false;
            if (trigger is KUIInteractionEventTrigger castEvent)
                return this.Start == castEvent.Start && this.Stop == castEvent.Stop;
            return false;
        }
    }
}