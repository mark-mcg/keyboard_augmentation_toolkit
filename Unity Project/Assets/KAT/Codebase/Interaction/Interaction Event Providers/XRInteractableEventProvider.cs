using KAT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KAT.Interaction
{
    public class XRInteractableEventProvider : XRBaseInteractable, IKUIInteractionProvider
    {
        protected KUILocationInteractionRegions interactionManager;
        private XRBaseInteractable xRBaseInteractable;
        KUIDockingManager dockingManager;
        KUIKeyLocationMetadata keyMetadata;
        public bool LogEvents = false;

        protected override void Awake()
        {
            base.Awake();
            if (xRBaseInteractable == null)
                xRBaseInteractable = this;

            if (colliders.Count == 0)
            {
                Collider collider = GetComponentInChildren<Collider>();
                if (collider != null)
                    colliders.Add(collider);
            }

            dockingManager = GetComponentInParent<KUIDockingManager>();
            keyMetadata = GetComponentInParent<KUIKeyLocationMetadata>();
            SetupXREventHandling(true);
        }

        public void RaiseEvent(BaseKUIEvent @event)
        {
            if (LogEvents) Debug.Log("<b>XRInteractableEventProvider</b> RaiseEvent called on " + this.gameObject + " event is " + @event);
            interactionManager.GetLocation().hub.Publish(@event);
        }

        public void SetLocationInteractionRegion(KUILocationInteractionRegions region)
        {
            interactionManager = region;
        }

        public virtual void OnDestroy()
        {
            SetupXREventHandling(false);
        }
        public virtual void SetupXREventHandling(bool listen)
        {
            if (xRBaseInteractable != null)
            {
                //Debug.Log("SetupXREventHandling listen=" + listen);
                if (listen)
                {
                    
                    xRBaseInteractable.onHoverExit.AddListener((interactor) => { RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.HoverExit, null, interactor)); });
                    xRBaseInteractable.onFirstHoverEnter.AddListener((interactor) => { RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.FirstHoverEnter, null, interactor)); });
                    xRBaseInteractable.onHoverEnter.AddListener((interactor) => { RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.HoverEnter, null, interactor)); });
                    xRBaseInteractable.onLastHoverExit.AddListener((interactor) => { RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.LastHoverExit, null, interactor)); });
                    xRBaseInteractable.onSelectEnter.AddListener((interactor) => { RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.SelectEnter, null, interactor)); });
                    xRBaseInteractable.onSelectExit.AddListener((interactor) => { RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.SelectExit, null, interactor)); });
                    xRBaseInteractable.onActivate.AddListener((interactor) => { RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.Activate, null, interactor)); });
                    xRBaseInteractable.onDeactivate.AddListener((interactor) =>
                    {
                        RaiseEvent(new KUIInteractionEvent(KUIInteractionEvent.Interaction.Inactive, null, interactor));
                    });

                    xRBaseInteractable.onActivate.AddListener((interactor) => {
                        if (!dockingManager.IsDocked)
                            keyMetadata.SimulateKeyPress(true, false);
                    });
                }
                else
                {
                    xRBaseInteractable.onHoverExit.RemoveAllListeners();
                    xRBaseInteractable.onFirstHoverEnter.RemoveAllListeners();
                    xRBaseInteractable.onHoverEnter.RemoveAllListeners();
                    xRBaseInteractable.onLastHoverExit.RemoveAllListeners();
                    xRBaseInteractable.onSelectEnter.RemoveAllListeners();
                    xRBaseInteractable.onSelectExit.RemoveAllListeners();
                    xRBaseInteractable.onActivate.RemoveAllListeners();
                    xRBaseInteractable.onDeactivate.RemoveAllListeners();
                    xRBaseInteractable.colliders.Clear();
                }
            } else
            {
                Debug.LogError("XRInteractableEventProvider cannot listen to events, xRBaseInteractable is null");
            }
        }

    }
}
