using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace KAT.Interaction 
{
    public abstract class KUIElementBaseTrigger<T> : KUIElementBaseTrigger
                where T : BaseKUIEvent
    {
        public override void SetKUIParent(KUIElement parent)
        {
            base.SetKUIParent(parent);

            parent.hub.Subscribe<T>(@event =>
            {
                //if (@event is T castEvent)
                //{
                    Debug.LogFormat("<color=blue>KUIElementBaseTrigger</color> GO:{0} class:{1} event:{2} eventtype:{3}", gameObject.name, this.GetType().Name, @event, @event.GetType().Name);
                    ProcessEvent(@event);
                //}
            });
        }

        public virtual void ProcessEvent(T @event)
        {

        }
    }

    public abstract class KUIElementBaseTrigger : KUIElementChild
    {
        [BoxGroup("KUIElementBaseTrigger")]
        public TriggerState TriggerState = new TriggerState();
        [BoxGroup("KUIElementBaseTrigger (Unity Events)")]
        public UnityEvent OnTriggerActive, OnTriggerInactive;
        [BoxGroup("KUIElementBaseTrigger (Unity Events)")]
        public UnityBoolEvent OnTriggerChanged;

        public void Start()
        {
            SetTriggerState(false);
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            SetTriggerState(false);
        }


        public override bool Equals(object other)
        {
            if (other is KUIElementBaseTrigger castTrigger)
            {
                if (castTrigger == null)
                {
                    return false;
                }
                else
                {
                    return Matches(castTrigger);
                }
            }
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract bool Matches(KUIElementBaseTrigger trigger);

        protected void SetTriggerState(bool triggered, bool repeatIfAlreadyTriggered = false)
        {
            //Debug.Log("SetTriggerState called for  " + triggered + " childactive " + ChildActive + " current state is " + TriggerState.IsTriggered());

            if (ChildActive && (TriggerState.SetTriggerState(triggered) || (triggered && repeatIfAlreadyTriggered) ))
            {
                Debug.Log("Raising events based on trigger state change " + triggered + " for " + this.gameObject);
                if (parent != null)
                {
                    parent.hub.Publish(new TriggerStateEvent(parent, TriggerState));
                    parent.hub.Publish(new KUIInteractionEvent(
                    triggered ? KUIInteractionEvent.Interaction.Activate : KUIInteractionEvent.Interaction.Inactive, parent));
                }

                if (triggered)
                    OnTriggerActive?.Invoke();
                else
                    OnTriggerInactive?.Invoke();

                OnTriggerChanged?.Invoke(triggered);
            }
        }

        protected void SetTriggerStateContinuous(float val)
        {
            if (ChildActive && TriggerState.SetAmount(val))
            {
                parent.hub.Publish(new TriggerStateEvent(parent, TriggerState));
                parent.hub.Publish(new KUIInteractionEvent(
                    TriggerState.IsTriggered() ? KUIInteractionEvent.Interaction.Activate : KUIInteractionEvent.Interaction.Inactive, parent
                ));
            }
        }
    }
}