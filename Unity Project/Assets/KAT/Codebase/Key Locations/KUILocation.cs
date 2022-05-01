using KAT.Layouts;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TriangleNet;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using KAT.Interaction;
using PubSub;

namespace KAT
{
    /// <summary>
    /// A virtual location tied to a KUILocationDescriptor e.g. the "a" key.
    /// 
    /// KUIElements are hosted here, added to their matching KUILocation. The KUILocation is informed
    /// of when there is an associated layout location. In effect, KUILocation binds KUIElements 
    /// to the layout location, and updates them when the layout changes.
    /// </summary>
    [RequireComponent(typeof(KUILocationDescriptor))]
    public class KUILocation : KUIParentChildEventProvider
    {
        public Hub hub = new Hub();
        private static bool LogEvents = true;
        public static KUILocationActiveElementSelector ElementSelector = new KUILocationActiveElementSelector();

        [ReadOnly]
        public List<KUIElement> AddedElements = new List<KUIElement>();
        public List<KUIElement> ActiveElements = new List<KUIElement>();

        public KUIManager kuiManager;
        public KUILocationDescriptor LayoutLocation;
        public KUILocationInteractionRegions InteractionRegions;
        public KUILocationDescriptor LocationTagCollection;

        public void Awake()
        {
            LocationTagCollection = GetComponent<KUILocationDescriptor>();

            if (LogEvents)
            {
                hub.Subscribe<BaseKUIEvent>(@event =>
                {
                    Debug.LogFormat("<color=blue>KUILocation</color> GO:{0} event:{1} eventtype:{2}", gameObject.name, @event, @event.GetType().Name);
                });
            }
        }

        public virtual void SetKUIManager(KUIManager manager)
        {
            Debug.Log("SetKUIManager");
            kuiManager = manager;

            // republish manager events
            kuiManager.KeyEventsHub.Subscribe<BaseKUIEvent>(this, @event =>
            {
                hub.Publish(@event);
            });

            hub.Subscribe<VisibilityStateEvent>(this, @event =>
            {
                // Debug.Log("Container received VisbilityStateEvent");
                UpdateActiveElementStates();
            });

            if (InteractionRegions == null)
            {
                InteractionRegions = KUILocationInteractionRegions.ConstructInteractionRegion(this);
            }
        }

        #region KUIElement management 
        public virtual void AddElement(KUIElement newShortcut, bool forceUsingThisShortcut = false, bool addSelfAsParentLocation = true, bool updateActiveElements = true)
        {
            //Debug.Log("AddElement called on " + this + " for " + newShortcut);
            if (!AddedElements.Contains(newShortcut))
            {
                AddedElements.Add(newShortcut);
                if (addSelfAsParentLocation) newShortcut.AddParentLocations(this);
                UpdateActiveElementStates();
                InteractionRegions.SetCollisionsActive(ActiveElements.Count != 0);
            }
        }

        public virtual void RemoveElement(KUIElement toRemove, bool removeSelfAsParentLocation = true)
        {
            //Debug.Log("Remove element called on " + this + " for " + toRemove);
            //Debug.Log("Location " + this + " removing shortcut " + toRemove);
            AddedElements.Remove(toRemove);
            if (removeSelfAsParentLocation) toRemove.RemoveParentLocations(this);
            UpdateActiveElementStates();
            InteractionRegions.SetCollisionsActive(ActiveElements.Count != 0);
        }

        public bool IsElementActiveAtThisLocation(KUIElement element)
        {
            return ActiveElements.Contains(element);
        }

        public virtual void UpdateActiveElementStates()
        {
            ActiveElements = ElementSelector.SelectActiveElements(this, AddedElements, ActiveElements);
            hub.Publish(new ActiveElementsChangedEvent());
        }
        #endregion


        #region Layout management
        public virtual void SetLayoutLocation(KUILocationDescriptor layoutLocation)
        {
            this.LayoutLocation = layoutLocation;
            this.transform.SetParent(LayoutLocation?.transform);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;

            // update the bounds so we can resize the interaction areas
            InteractionRegions.UpdateBounds(layoutLocation.gameObject.GetComponentInChildren<KUILocationBounds>());
        }
        #endregion

        #region KUIElement helpers
        public List<Collider> GetLocationColliders()
        {
            return this.GetComponentsInChildren<Collider>().ToList();
        }
        #endregion

    }
}