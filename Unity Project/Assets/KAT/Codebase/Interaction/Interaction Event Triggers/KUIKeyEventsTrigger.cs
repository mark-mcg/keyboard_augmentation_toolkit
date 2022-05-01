using Boo.Lang;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;

namespace KAT.Interaction
{
    [Serializable]
    public class UnityKeyEvent : UnityEvent<KUIKeyEventsTrigger> {     }

    [RequireComponent(typeof(KUIKeyElementVisibilityController))]
    public class KUIKeyEventsTrigger : KUIElementBaseTrigger<KeyHookEvent>
    {
        [BoxGroup("KUIKeyEventTrigger")]
        public KeyEventTrigger[] triggers;
        [BoxGroup("KUIKeyEventTrigger")]
        public bool consumeKeyPress = false;
        [BoxGroup("KUIKeyEventTrigger (Unity events)")]
        public UnityKeyEvent OnKeyEvent;

        public override bool Matches(KUIElementBaseTrigger trigger)
        {
            if (trigger == null)
                return false;
            if (trigger is KUIKeyEventsTrigger castEvent)
                return triggers.All(x => castEvent.triggers.Contains(x) && triggers.Length == castEvent.triggers.Length);
            return false;
        }

        public override void ProcessEvent(KeyHookEvent castEvent)
        {
            bool allKeysActive = triggers.Any( x => x.IsPressed() );
            //Debug.Log("Triggers are " + string.Join(",", triggers.AsEnumerable()) + " allKeysActive=" + allKeysActive);

            SetTriggerState(triggers.Any(x => x.key.Equals(castEvent.key)) && allKeysActive, !castEvent.Up);

            if (triggers.Any(x => x.key.Equals(castEvent.key)) && allKeysActive)
            {
                OnKeyEvent?.Invoke(this);

                if (consumeKeyPress)
                    castEvent.NoteConsumed();
            }
        }

        public override string ToString()
        {
            return string.Format("KUIKeyEventTrigger");

        }
    }
}