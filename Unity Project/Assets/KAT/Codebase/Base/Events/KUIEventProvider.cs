using NaughtyAttributes;
using UnityEngine;

namespace KAT
{
    public class KUIEventProvider : MonoBehaviour//, KUIEvents.KUIEventProvider, KUIEvents.KUIEventReceiver
    {
        //[BoxGroup("KUIEventProvider")]
        //public bool LogProviderEvents = false;
        //public event KUIEvents.KUIEventDelegate OnKUIEvent;

        //public void AddListener(KUIEvents.KUIEventReceiver listener)
        //{
        //    OnKUIEvent += listener.ReceiveKUIEvent;
        //}

        //public void RemoveListener(KUIEvents.KUIEventReceiver listener)
        //{
        //    OnKUIEvent -= listener.ReceiveKUIEvent;
        //}

        //public virtual void ReceiveKUIEvent(BaseKUIEvent @event)
        //{
        //}

        //public void RaiseEvent(BaseKUIEvent @event)
        //{
        //    if (LogProviderEvents) Debug.Log("RaiseEvent called on " + this.gameObject + " event is " + @event);
        //    if (OnKUIEvent != null) OnKUIEvent(@event);
        //}
    }

    public class KUIParentChildEventProvider : MonoBehaviour, KUIEvents.KUIElementDepedent
    {
        protected KUIElement kuiElementParent;
        public virtual void SetKUIParent(KUIElement parent)
        {
            kuiElementParent = parent;
        }
    }
}
