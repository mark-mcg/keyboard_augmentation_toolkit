using Boo.Lang;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace KAT.Interaction
{
    public class UnityTapEvent : UnityEvent<TapType> { }

    public class KUITapEventsTrigger : KUIElementBaseTrigger<TapEvent>
    {
        [BoxGroup("Tap Events")]
        public bool ActivateOnTap = true;
        [BoxGroup("Tap Events")]
        public TapType ActivateOn = TapType.Single;
        [BoxGroup("Tap Events (Unity)")]
        public UnityEvent OnSingleTap, OnDoubleTap, OnTripleTap, OnTapExpiry;
        [BoxGroup("Tap Events (Unity)")]
        public UnityTapEvent OnTap;

        public override bool Matches(KUIElementBaseTrigger trigger)
        {
            if (trigger == null)
                return false;
            if (trigger is KUITapEventsTrigger castEvent)
                return this.ActivateOn == castEvent.ActivateOn && this.ActivateOnTap == castEvent.ActivateOnTap;
            return false;
        }

        public override void ProcessEvent(TapEvent castEvent)
        {
            Debug.Log("ReceiveKUIEvent for KUITapEventsTrigger " + castEvent);

            SetTriggerState(castEvent.TapType == ActivateOn && ActivateOnTap);

            switch (castEvent.TapType)
            {
                case TapType.Single:
                    OnSingleTap?.Invoke();
                    break;
                case TapType.Double:
                    OnDoubleTap?.Invoke();
                    break;
                case TapType.Triple:
                    OnTripleTap?.Invoke();
                    break;
                case TapType.None:
                    OnTapExpiry?.Invoke();
                    break;
            }

            OnTap?.Invoke(castEvent.TapType);
        }

        public override string ToString()
        {
            return string.Format("KUITapEventsTrigger");

        }
    }
}