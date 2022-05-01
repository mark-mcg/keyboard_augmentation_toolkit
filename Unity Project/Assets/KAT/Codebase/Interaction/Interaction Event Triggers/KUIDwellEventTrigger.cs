using Boo.Lang;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace KAT.Interaction
{
    public class KUIDwellEventTrigger : KUIElementBaseTrigger<DwellEvent>
    {
        [BoxGroup("Dwell Events")]
        public bool ActivateOnDwell = true;
        [BoxGroup("Dwell Events (Unity)")]
        public UnityEvent OnDwell;
        [BoxGroup("Dwell Events (Unity)")]
        public UnityBoolEvent OnDwellToggle;

        public override void ProcessEvent(DwellEvent castEvent)
        {
            base.ProcessEvent(castEvent);
            SetTriggerState(ActivateOnDwell);
            if (castEvent.Dwelling)
            {
                OnDwell?.Invoke();
            }
            OnDwellToggle?.Invoke(castEvent.Dwelling);
        }

        public override bool Matches(KUIElementBaseTrigger trigger)
        {
            if (trigger == null)
                return false;
            if (trigger is KUIDwellEventTrigger castEvent)
                return this.ActivateOnDwell == castEvent.ActivateOnDwell;
            return false;
        }

        public override string ToString()
        {
            return string.Format("KUIDwellEventTrigger");
        }
    }
}